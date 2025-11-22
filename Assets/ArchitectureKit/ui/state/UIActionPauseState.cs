using UnityEngine.UI;

namespace Namespace_UIPause
{
    internal sealed class UIActionPauseState
    {
        private readonly IEventBus _bus;
        private readonly IUIManager _ui;

        public UIActionPauseState(IEventBus bus, IUIManager ui)
        {
            _bus = bus;
            _ui = ui;
        }

        public void OnPauseEnter()
        {
            _ui.IShow("ui-pause");

            var btn = _ui.IGetComponentInUI<Button>("ui-pause", "btn-resume");

            if (!btn) return;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => _bus.IPublish(new RequestStateEnter("gameplay_state")));
        }

        public void OnPauseExit()
        {
            _ui.IHide("ui-pause");
        }
    }
}
