using UnityEngine;
using UnityEngine.UI;
using Namespace_StateLobby_Event;
using PurrLobby;
using System.Collections.Generic;
using Namespace_StateMainMenu_Event;
using Namespace_Level;

namespace Namespace_UILobby
{
    internal sealed class UIActionLobbyState
    {
        private readonly IUIManager _ui;
        private readonly IEventBus _bus;
        private readonly LobbyManager _lobbyManager;

        // Names defined in UICatalog
        private const string UI_ROOM = "ui-lobby-room";

        public UIActionLobbyState(IUIManager ui, IEventBus bus, LobbyManager lobbyManager)
        {
            _ui = ui;
            _bus = bus;
            _lobbyManager = lobbyManager;
        }

        public void OnLobbyEnter()
        {
            Debug.Log("Entered lobby state");

            // 1. Subscribe to LobbyManager events
            _lobbyManager.OnRoomLeft.AddListener(CallOnRoomLeft);
            _lobbyManager.OnRoomUpdated.AddListener(CallOnRoomUpdated);
            _lobbyManager.OnAllReady.AddListener(CallOnAllReady);
            // 2. Show the Room UI
            _ui.IShow(UI_ROOM);

            // 3. Update UI immediately if data exists
            if (_lobbyManager.CurrentLobby.IsValid)
            {
                UpdateRoomUI(_lobbyManager.CurrentLobby);
            }
        }

        public void OnLobbyExit()
        {
            // 1. Hide UI
            _ui.IHide(UI_ROOM);

            // 2. Unsubscribe events
            _lobbyManager.OnRoomLeft.RemoveListener(CallOnRoomLeft);
            _lobbyManager.OnRoomUpdated.RemoveListener(CallOnRoomUpdated);
            _lobbyManager.OnAllReady.AddListener(CallOnAllReady);
        }

        #region EVENT CALLBACKS

        private void CallOnRoomLeft()
        {
            // When we leave the room (logic handled by LobbyManager), we transition back to Main Menu state
            _bus.IPublish(new RequestStateEnter("mainmenu_state"));
            _bus.IPublish(new LevelRequest("mainmenu_scene"));
            // Note: Depending on your StateMachine implementation, you might use 'RequestStateEnter("mainmenu_state")' instead
            // But based on your imports, this event seems to trigger the flow.
        }

        private void CallOnRoomUpdated(Lobby lobby)
        {
            UpdateRoomUI(lobby);
        }

        private void UpdateRoomUI(Lobby lobby)
        {
            var roomGo = _ui.IGet(UI_ROOM);
            if (!roomGo || !roomGo.activeInHierarchy) return;

            // Update Member List using your existing helper script
            var memberList = roomGo.GetComponentInChildren<LobbyMemberList>();
            if (memberList) memberList.LobbyDataUpdate(lobby);

            // Bind Room Buttons
            BindButton(UI_ROOM, "btn-ready", () => _lobbyManager.ToggleLocalReady());
            BindButton(UI_ROOM, "btn-leave", () => _lobbyManager.LeaveLobby());

            // Example: Update Room Name Text
            // var title = _ui.IGetComponentInUI<TMPro.TMP_Text>(UI_ROOM, "Text_RoomName");
            // if (title) title.text = lobby.Name;
        }

        private void CallOnAllReady()
        {
            Debug.Log("<color=green>OnAllReady");
            _bus.IPublish(new RequestStateEnter("gameplay_state"));
            _bus.IPublish(new LevelRequest("gameplay_scene"));
        }

        #endregion

        // Helper to find button and add listener safely
        private void BindButton(string uiName, string childPath, UnityEngine.Events.UnityAction action)
        {
            var btn = _ui.IGetComponentInUI<Button>(uiName, childPath);
            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
            }
        }
    }
}