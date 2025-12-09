using Namespace_Level;
using PurrLobby;
using UnityEngine.UI;

namespace Namespace_UIPause
{
    internal sealed class UIActionPauseState
    {
        private readonly IEventBus _bus;
        private readonly IUIManager _ui;
        private readonly LobbyManager _lobbyManager;

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

            var buttons = _ui.IGetAllComponentInUI<Button>("ui-pause");
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
        }

        private void CallOnRoomLeft()
        {
            _bus.IPublish(new RequestStateEnter("mainmenu_state"));
            _bus.IPublish(new LevelRequest("mainmenu_scene"));
        }

        public void OnPauseExit()
        {
            _ui.IHide("ui-pause");
            _lobbyManager.OnRoomLeft.RemoveListener(CallOnRoomLeft);
        }
    }
}
