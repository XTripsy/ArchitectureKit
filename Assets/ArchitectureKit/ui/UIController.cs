using UIMainMenu;

public sealed class UIController
{
    private readonly IEventBus _bus;
    private readonly IUIManager _ui;

    public UIController(IEventBus temp_bus, IUIManager temp_ui)
    {
        _bus = temp_bus; 
        _ui = temp_ui;

        UIActionMainMenuState temp = new UIActionMainMenuState(_bus, _ui);
        _bus.ISubscribe<MainMenuStateEnter>( _=> temp.OnMainMenuEnter());
        _bus.ISubscribe<MainMenuStateExit>( _=> temp.OnMainMenuExit());
    }
}
