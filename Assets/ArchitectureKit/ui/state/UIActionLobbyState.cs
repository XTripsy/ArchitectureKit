using TMPro;
using PurrLobby;
using UnityEngine;
using UnityEngine.UI;
using Namespace_Level;
using System.Collections.Generic;

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
            _lobbyManager.OnFriendListPulled.AddListener(CallOnFriendsListPulled);


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
            _lobbyManager.OnAllReady.RemoveListener(CallOnAllReady);
            _lobbyManager.OnFriendListPulled.RemoveListener(CallOnFriendsListPulled);
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

            var memberList = roomGo.GetComponentInChildren<LobbyMemberList>();
            if (memberList) memberList.LobbyDataUpdate(lobby);

            LobbyCode();

            BindButton(UI_ROOM, "btn-ready", () => _lobbyManager.ToggleLocalReady());
            BindButton(UI_ROOM, "btn-leave", () => _lobbyManager.LeaveLobby());
            BindButton(UI_ROOM, "btn-copy", () => CopyCode());
        }


        private void CallOnAllReady()
        {
            Debug.Log("<color=green>CallOnAllReady : OnAllReady");
            _bus.IPublish(new RequestStateEnter("gameplay_state"));
        }

        private void CallOnFriendsListPulled(List<FriendUser> list)
        {
            var friendsList = _ui.IGetComponentInUI<CustomFriendList>(UI_ROOM, "list-friends");

            if (friendsList != null)
                friendsList.Populate(list);
            else
                Debug.LogError("CallOnFriendsListPulled(): CustomFriendList is null");
        }

        #endregion

        public void CopyCode()
        {
            GUIUtility.systemCopyBuffer = roomId;
            Debug.Log("<color=green>CODE COPIED");
        }

        private void BindButton(string uiName, string buttonName, UnityEngine.Events.UnityAction action)
        {
            var allButtons = _ui.IGetAllComponentInUI<Button>(uiName);

            if (allButtons == null)
            {
                Debug.LogWarning($"No buttons found in UI: {uiName}");
                return;
            }

            foreach (var btn in allButtons)
            {
                if (btn.gameObject.name == buttonName)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(action);
                    return;
                }
            }

            Debug.LogWarning($"Button '{buttonName}' not found in '{uiName}'");
        }
    }
}