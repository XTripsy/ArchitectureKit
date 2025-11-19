using UnityEngine.UI;
using UnityEngine;

public sealed class UIActionMainMenuState
{
    private readonly IEventBus _bus;
    private readonly IUI _ui;

    public UIActionMainMenuState(IEventBus bus, IUI ui)
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
        btn.onClick.AddListener(() => _bus.IPublish(new SLevelRequest("gameplay_scene")));
        btn.onClick.AddListener(() => _bus.IPublish(new RequestGameplayStateEnter()));
    }

    public void OnMainMenuExit()
    {
        _ui.IHide("ui-mainmenu");
    }
}
