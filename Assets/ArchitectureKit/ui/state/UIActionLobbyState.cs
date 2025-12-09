using TMPro;
using PurrLobby;
using UnityEngine;
using UnityEngine.UI;
using Namespace_Level;
using System.Collections;
using Namespace_StateLobby_Event;
using System.Collections.Generic;
using Namespace_StateMainMenu_Event;

namespace Namespace_UILobby
{
    internal sealed class UIActionLobbyState
    {
        private readonly IUIManager _ui;
        private readonly IEventBus _bus;
        private readonly LobbyManager _lobbyManager;
        private const string UI_ROOM = "ui-lobby-room";
        private string roomId;

        public UIActionLobbyState(IUIManager ui, IEventBus bus, LobbyManager lobbyManager)
        {
            _ui = ui;
            _bus = bus;
            _lobbyManager = lobbyManager;
        }

        public void OnLobbyEnter()
        {
            Debug.Log("Entered lobby state");

            _lobbyManager.OnRoomLeft.AddListener(CallOnRoomLeft);
            _lobbyManager.OnRoomUpdated.AddListener(CallOnRoomUpdated);
            _lobbyManager.OnAllReady.AddListener(CallOnAllReady);

            _ui.IShow(UI_ROOM);

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

        private void LobbyCode()
        {
            var texts = _ui.IGetAllComponentInUI<TMP_Text>(UI_ROOM);
            foreach (var text in texts)
            {
                switch (text.gameObject.name)
                {
                    case "txt-code":
                        roomId = _lobbyManager.CurrentLobby.LobbyId;
                        text.text = roomId;
                        break;
                }
            }


        }

        #region EVENT CALLBACKS

        private void CallOnRoomLeft()
        {
            _bus.IPublish(new RequestStateEnter("mainmenu_state"));
            _bus.IPublish(new LevelRequest("mainmenu_scene"));
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

            //set lobby code
            LobbyCode();

            // Bind Room Buttons
            BindButton(UI_ROOM, "btn-ready", () => _lobbyManager.ToggleLocalReady());
            BindButton(UI_ROOM, "btn-leave", () => _lobbyManager.LeaveLobby());
            BindButton(UI_ROOM, "btn-copy", () => CopyCode());
        }

        private void CallOnAllReady()
        {
            Debug.Log("<color=green>OnAllReady");
            _bus.IPublish(new RequestStateEnter("gameplay_state"));
            // _bus.IPublish(new LevelRequest("gameplay_scene"));
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

        public void CopyCode()
        {
            GUIUtility.systemCopyBuffer = roomId;
            Debug.Log("<color=green>CODE COPIED");
        }
    }
}