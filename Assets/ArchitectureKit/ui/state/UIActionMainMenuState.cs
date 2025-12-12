using UnityEngine;
using UnityEngine.UI;
using PurrLobby;
using System.Collections.Generic;
using Namespace_Level;
using PurrNet;
using TMPro;

namespace Namespace_UIMainMenu
{
    internal sealed class UIActionMainMenuState
    {
        private readonly IEventBus _bus;
        private readonly IUIManager _ui;
        private readonly LobbyManager _lobbyManager;

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
            _lobbyManager.OnRoomJoined.AddListener(CallOnRoomJoined);
            _lobbyManager.OnRoomSearchResults.AddListener(CallOnSearchResults);
            _lobbyManager.OnRoomJoinFailed.AddListener(CallOnJoinFailed);

            ShowMainScreen();
        }

        public void OnMainMenuExit()
        {
            _ui.IHide(UI_MAIN);
            _ui.IHide(UI_BROWSE);
            _ui.IHide(UI_CREATE);

            _lobbyManager.OnRoomJoined.RemoveListener(CallOnRoomJoined);
            _lobbyManager.OnRoomSearchResults.RemoveListener(CallOnSearchResults);
            _lobbyManager.OnRoomJoinFailed.RemoveListener(CallOnJoinFailed);
        }

        private void ShowMainScreen()
        {
            _ui.IHide(UI_BROWSE);
            _ui.IHide(UI_CREATE);
            _ui.IHide(UI_LOADING);
            _ui.IShow(UI_MAIN);

            BindButton(UI_MAIN, "btn-browse", () =>
            {
                ShowBrowseScreen();
                _lobbyManager.SearchLobbies();
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

            SetLobbyList();
        }

        private void SetLobbyList()
        {
            _bus.ISubscribe<LevelLoad>(e =>
            {
                if (e.level != "mainmenu_scene") return;
                CustomLobbyList list = Object.FindFirstObjectByType<CustomLobbyList>();

                if (list == null)
                {
                    Debug.LogWarning("lobby list null, initializing browse ui first");
                    _ui.IShow("ui-lobby-browse");
                    _ui.IHide("ui-lobby-browse");
                    list = Object.FindFirstObjectByType<CustomLobbyList>();
                    list?.Init(_bus, _lobbyManager);
                }
                list?.Init(_bus, _lobbyManager);
            });
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
                        break;
                }
            }
        }

        #region EVENT CALLBACKS

        private void CallOnSearchResults(List<Lobby> lobbies)
        {
            var browseGo = _ui.IGet(UI_BROWSE);
            if (!browseGo) return;

            var listScript = browseGo.GetComponentInChildren<CustomLobbyList>();
            if (listScript)
            {
                listScript.Populate(lobbies);
            }
        }

        private void CallOnRoomJoined(Lobby lobby)
        {
            _bus.IPublish(new RequestStateEnter("lobby_state"));
            _bus.IPublish(new LevelRequest("gameplay_scene"));
        }

        private void CallOnJoinFailed(string error)
        {
            _ui.IHide(UI_LOADING);
            Debug.LogError($"Join Failed: {error}");
        }

        #endregion

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