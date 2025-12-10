using Namespace_Level;
using PurrLobby;
using UnityEngine;
using UnityEngine.UI;

namespace Namespace_UIPause
{
    internal sealed class UIActionPauseState
    {
        private readonly IEventBus _bus;
        private readonly IUIManager _ui;
        private readonly LobbyManager _lobbyManager;
        private readonly string UI_PAUSE = "ui-pause";

        public UIActionPauseState(IEventBus bus, IUIManager ui, LobbyManager lm)
        {
            _bus = bus;
            _ui = ui;
            _lobbyManager = lm;
        }

        public void OnPauseEnter()
        {
            _ui.IShow("ui-pause");
            _lobbyManager.OnRoomLeft.AddListener(CallOnRoomLeft);

            var buttons = _ui.IGetAllComponentInUI<Button>(UI_PAUSE);
            if (buttons == null) return;
            foreach (var btn in buttons)
            {
                btn.onClick.RemoveAllListeners();
                switch (btn.gameObject.name)
                {
                    case "btn-resume":
                        btn.onClick.AddListener(() => _bus.IPublish(new RequestStateEnter("gameplay_state")));
                        break;
                    case "btn-leave":
                        btn.onClick.AddListener(_lobbyManager.LeaveLobby);
                        break;
                }
            }

            // BindButton(UI_PAUSE, "btn-resume", () => _bus.IPublish(new RequestStateEnter("gameplay_state")));
            // BindButton(UI_PAUSE, "btn-leave", () => _lobbyManager.LeaveLobby());
        }

        private void CallOnRoomLeft()
        {
            Debug.LogWarning("Room Left");
            _bus.IPublish(new RequestStateEnter("mainmenu_state"));
            _bus.IPublish(new LevelRequest("mainmenu_scene"));
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

        public void OnPauseExit()
        {
            _ui.IHide("ui-pause");
            _lobbyManager.OnRoomLeft.RemoveListener(CallOnRoomLeft);
        }
    }
}
