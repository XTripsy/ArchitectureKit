using Namespace_Level;
using UnityEngine.UI;

namespace Namespace_UILobby
{
    internal sealed class UIActionLobbyState
    {
        private readonly IUIManager _ui;
        private readonly IEventBus _bus;

        public UIActionLobbyState(IEventBus bus, IUIManager ui)
        {
            _ui = ui;
            _bus = bus;
        }

        public void OnLobbyEnter()
        {
            _ui.IShow("ui-lobby");

            var btn = _ui.IGetComponentInUI<Button>("ui-lobby", "btn-leave");

            if (!btn) return;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => _bus.IPublish(new LevelRequest("mainmenu_scene")));
            btn.onClick.AddListener(() => _bus.IPublish(new RequestStateEnter("mainmenu_state")));
        }

        public void OnLobbyExit()
        {
            _ui.IHide("ui-lobby");
        }
    }
}
