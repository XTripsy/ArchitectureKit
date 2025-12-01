using UnityEngine;
using UnityEngine.UI;
using Namespace_StateLobby_Event;
using PurrLobby;
using System.Collections.Generic;
using Namespace_StateMainMenu_Event;

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
            // 1. Subscribe to LobbyManager events
            _lobbyManager.OnRoomLeft.AddListener(OnLeftRoom);
            _lobbyManager.OnRoomUpdated.AddListener(OnLobbyUpdated);

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
            _lobbyManager.OnRoomLeft.RemoveListener(OnLeftRoom);
            _lobbyManager.OnRoomUpdated.RemoveListener(OnLobbyUpdated);
        }

        // --- Event Callbacks ---

        private void OnLeftRoom()
        {
            // When we leave the room (logic handled by LobbyManager), we transition back to Main Menu state
            _bus.IPublish(new MainMenuStateEnter());
            // Note: Depending on your StateMachine implementation, you might use 'RequestStateEnter("mainmenu_state")' instead
            // But based on your imports, this event seems to trigger the flow.
        }

        private void OnLobbyUpdated(Lobby lobby)
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
            BindButton(UI_ROOM, "Btn_Ready", () => _lobbyManager.ToggleLocalReady());
            BindButton(UI_ROOM, "Btn_Leave", () => _lobbyManager.LeaveLobby());

            // Example: Update Room Name Text
            // var title = _ui.IGetComponentInUI<TMPro.TMP_Text>(UI_ROOM, "Text_RoomName");
            // if (title) title.text = lobby.Name;
        }

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