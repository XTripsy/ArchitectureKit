using UnityEngine.UI;
using Namespace_Level;

namespace Namespace_UIMainMenu
{
    internal sealed class UIActionMainMenuState
    {
        private readonly IEventBus _bus;
        private readonly IUIManager _ui;

        public UIActionMainMenuState(IEventBus bus, IUIManager ui)
        {
            _bus = bus;
            _ui = ui;
        }

        public void OnMainMenuEnter()
        {
            _ui.IShow("ui-mainmenu");

            var btn = _ui.IGetComponentInUI<Button>("ui-mainmenu", "btn-play");

            if (!btn) return;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => _bus.IPublish(new LevelRequest("gameplay_scene")));
            btn.onClick.AddListener(() => _bus.IPublish(new RequestStateEnter("gameplay_state")));
        }

        public void OnMainMenuExit()
        {
            _ui.IHide("ui-mainmenu");
        }
    }
}
