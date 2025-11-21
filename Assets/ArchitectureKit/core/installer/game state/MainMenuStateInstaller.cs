using InputMainMenu;
using MyInput;
using UIMainMenu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State_MainMenu
{
    internal sealed class MainMenuStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            string name_state = "mainmenu_state";
            IEventBus bus = installer.IGetBus;
            IStateRegistry state = installer.IGetStateRegistry;
            IInputManager input = installer.IResolve<IInputManager>();
            IUIManager ui = installer.IResolve<IUIManager>();

            state.IRegister(name_state, new MainMenuState(bus));

            InputGroup group = installer.IGetGroup<InputGroup>();
            int index = input.IGetIndexCatalogInputAction("mainmenu_mapping", group.catalog);
            InputActionMap inputAction = group.action.FindActionMap("mainmenu_mapping", throwIfNotFound: false);
            InputCatalog.Mapping mapping = group.catalog.InputAction[index];
            IAction action = new ActionMainMenuState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_state, action);

            InputActionMainMenuState temp_input = new InputActionMainMenuState(bus);
            bus.ISubscribe<ActionPlayMainMenuState>(_ => temp_input.PlayMainMenu());

            UIActionMainMenuState temp_ui = new UIActionMainMenuState(bus, ui);
            bus.ISubscribe<MainMenuStateEnter>(_ => temp_ui.OnMainMenuEnter());
            bus.ISubscribe<MainMenuStateExit>(_ => temp_ui.OnMainMenuExit());
        }
    }
}