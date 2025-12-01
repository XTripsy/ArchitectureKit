using UnityEngine.UI;
using UnityEngine;
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

            var buttons = _ui.IGetAllComponentInUI<Button>("ui-mainmenu");

            foreach (var btn in buttons)
            {
                btn.onClick.RemoveAllListeners();
                switch (btn.gameObject.name)
                {
                    case "btn-create":
                        btn.onClick.AddListener(() => _bus.IPublish(new LevelRequest("lobby_scene")));
                        btn.onClick.AddListener(() => _bus.IPublish(new RequestStateEnter("lobby_state")));
                        break;
                    case "btn-browse":
                        btn.onClick.AddListener(() =>
                        {
                            Debug.Log("<color=yellow> BROWSE");
                        });
                        break;
                    case "btn-join":
                        btn.onClick.AddListener(() =>
                        {
                            Debug.Log("<color=blue> JOIN");
                        });
                        break;
                }
            }
        }

        public void OnMainMenuExit()
        {
            _ui.IHide("ui-mainmenu");
        }
    }
}
