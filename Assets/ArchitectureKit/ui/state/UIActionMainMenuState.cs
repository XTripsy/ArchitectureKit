using UnityEngine;
using UnityEngine.UI;
using Namespace_StateMainMenu_Event;
using PurrLobby;
using System.Collections.Generic;
using Namespace_Level;
using TMPro;

namespace Namespace_UIMainMenu
{
    internal sealed class UIActionMainMenuState
    {
        private readonly IEventBus _bus;
        private readonly IUIManager _ui;
        private readonly LobbyManager _lobbyManager;

        // UI IDs from UICatalog
        private const string UI_MAIN = "ui-mainmenu";
        private const string UI_BROWSE = "ui-lobby-browse";
        private const string UI_CREATE = "ui-lobby-create";
        private const string UI_LOADING = "ui-loading";

        public UIActionMainMenuState(IEventBus bus, IUIManager ui, LobbyManager lobbyManager)
        {
            _bus = bus;
            _ui = ui;
            _lobbyManager = lobbyManager;
        }

        public void OnMainMenuEnter()
        {
            // 1. Subscribe to LobbyManager events
            _lobbyManager.OnRoomJoined.AddListener(CallOnRoomJoined);
            _lobbyManager.OnRoomSearchResults.AddListener(CallOnSearchResults);
            _lobbyManager.OnRoomJoinFailed.AddListener(CallOnJoinFailed);

            // 2. Start at Root Screen
            ShowMainScreen();
        }

        public void OnMainMenuExit()
        {
            // Hide All
            _ui.IHide(UI_MAIN);
            _ui.IHide(UI_BROWSE);
            _ui.IHide(UI_CREATE);
            _ui.IHide(UI_LOADING);

            // 2. Unsubscribe
            _lobbyManager.OnRoomJoined.RemoveListener(CallOnRoomJoined);
            _lobbyManager.OnRoomSearchResults.RemoveListener(CallOnSearchResults);
            _lobbyManager.OnRoomJoinFailed.RemoveListener(CallOnJoinFailed);
        }

        // --- Screen Navigation ---

        private void ShowMainScreen()
        {
            _ui.IHide(UI_BROWSE);
            _ui.IHide(UI_CREATE);
            _ui.IHide(UI_LOADING);
            _ui.IShow(UI_MAIN);

            BindButton(UI_MAIN, "btn-browse", () =>
            {
                ShowBrowseScreen();
                _lobbyManager.SearchLobbies(); // Auto-search on open
                Debug.Log("<color=green>searching for lobbies</color>");
            });

            BindButton(UI_MAIN, "btn-create", ShowCreateScreen);
            BindButton(UI_MAIN, "btn-quit", () => Application.Quit());
            BindButton(UI_MAIN, "btn-join", () => JoinRoom());
        }

        private void ShowBrowseScreen()
        {
            _ui.IHide(UI_MAIN);
            _ui.IHide(UI_CREATE);
            _ui.IShow(UI_BROWSE);

            BindButton(UI_BROWSE, "btn-back", ShowMainScreen);
            BindButton(UI_BROWSE, "btn-refresh", () => _lobbyManager.SearchLobbies());
        }

        private void ShowCreateScreen()
        {
            _ui.IHide(UI_MAIN);
            _ui.IShow(UI_CREATE);

            BindButton(UI_CREATE, "btn-cancel", ShowMainScreen);
            BindButton(UI_CREATE, "btn-confirm", () =>
            {
                _ui.IShow(UI_LOADING);
                _lobbyManager.CreateRoom();
            });
        }

        private void JoinRoom()
        {
            var inputfields = _ui.IGetAllComponentInUI<TMP_InputField>(UI_MAIN);

            foreach (var inputfield in inputfields)
            {
                switch (inputfield.gameObject.name)
                {
                    case "input-code":
                        _lobbyManager.JoinLobby(inputfield.text);
                        Debug.Log($"<color=green> joining room {inputfield}");
                        break;
                }
            }
        }

        #region EVENT CALLBACKS

        private void CallOnSearchResults(List<Lobby> lobbies)
        {
            var browseGo = _ui.IGet(UI_BROWSE);
            if (!browseGo) return;

            // Use the existing LobbyList script on the prefab to populate UI
            var listScript = browseGo.GetComponentInChildren<LobbyList>();
            if (listScript)
            {
                listScript.Populate(lobbies);
            }
        }

        private void CallOnRoomJoined(Lobby lobby)
        {
            // Successful join -> Request transition to Lobby State
            _bus.IPublish(new RequestStateEnter("lobby_state"));
            _bus.IPublish(new LevelRequest("gameplay_scene"));
            Debug.Log("<color=green>Room Joined");
        }

        private void CallOnJoinFailed(string error)
        {
            _ui.IHide(UI_LOADING);
            Debug.LogError($"Join Failed: {error}");
            // Optional: Show error popup here
        }

        #endregion

        private void BindButton(string uiName, string buttonName, UnityEngine.Events.UnityAction action)
        {
            // Get all buttons in the UI canvas/panel
            var allButtons = _ui.IGetAllComponentInUI<Button>(uiName);

            if (allButtons == null)
            {
                Debug.LogWarning($"No buttons found in UI: {uiName}");
                return;
            }

            // Iterate to find the one with the matching name
            foreach (var btn in allButtons)
            {
                if (btn.gameObject.name == buttonName)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(action);
                    return; // Found and bound, exit method
                }
            }

            Debug.LogWarning($"Button '{buttonName}' not found in '{uiName}'");
        }
    }
}