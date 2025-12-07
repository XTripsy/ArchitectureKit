using UnityEngine.InputSystem;
using Namespace_InputMainMenu;
using Namespace_InputMainMenu_Event;
using Namespace_UIMainMenu;
using Namespace_StateMainMenu_Event;
using Namespace_Input;
using PurrLobby;
using Namespace_Level;
using UnityEngine;

namespace Namespace_StateMainMenu
{
    internal sealed class MainMenuStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            string name_state = "mainmenu_state";
            string name_mapping = "mainmenu_mapping";

            IEventBus bus = installer.IGetBus;
            IStateRegistry state = installer.IGetStateRegistry;
            IInputManager input = installer.IResolve<IInputManager>();
            IUIManager ui = installer.IResolve<IUIManager>();
            IGameLoopManager gameLoopManager = installer.IResolve<IGameLoopManager>();
            LobbyManager lobbyManager = installer.IResolve<LobbyManager>();

            bus.ISubscribe<LevelLoad>(e =>
            {
                if (e.level != "mainmenu_scene") return;
                CustomLobbyList list = Object.FindFirstObjectByType<CustomLobbyList>();

                if (!list)
                {
                    Debug.LogWarning("lobby list null, initializing browse ui first");
                    ui.IShow("ui-lobby-browse");
                    ui.IHide("ui-lobby-browse");
                    list = Object.FindFirstObjectByType<CustomLobbyList>();
                    list?.Init(bus, lobbyManager);
                }
                list?.Init(bus, lobbyManager);
            });

            state.IRegister(name_state, new MainMenuState(bus));

            _InstallInput(installer, bus, input, name_mapping);
            _InstallInputAction(bus);
            _InstallUI(bus, ui, lobbyManager);
        }

        private void _InstallInput(IBootstrapContext installer, IEventBus bus, IInputManager input, string name_mapping)
        {
            InputGroup group = installer.IGetGroup<InputGroup>();
            int index = input.IGetIndexCatalogInputAction(name_mapping, group.catalog);
            InputActionMap inputAction = group.action.FindActionMap(name_mapping, throwIfNotFound: false);
            InputCatalog.Mapping mapping = group.catalog.InputAction[index];
            IAction action = new ActionMainMenuState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_mapping, action);

            bus.ISubscribe<MainMenuStateEnter>(_ => input.IActiveActionInput(name_mapping));
        }

        private void _InstallInputAction(IEventBus bus)
        {
            InputActionMainMenuState temp_input = new InputActionMainMenuState(bus);
            bus.ISubscribe<ActionPlayMainMenuState>(_ => temp_input.CreateRoomMainMenu());
            bus.ISubscribe<ActionBrowseMainMenuState>(_ => temp_input.JoinMainMenu());
        }

        private void _InstallUI(IEventBus bus, IUIManager ui, LobbyManager lobbyManager)
        {
            UIActionMainMenuState temp_ui = new UIActionMainMenuState(bus, ui, lobbyManager);
            bus.ISubscribe<MainMenuStateEnter>(_ => temp_ui.OnMainMenuEnter());
            bus.ISubscribe<MainMenuStateExit>(_ => temp_ui.OnMainMenuExit());
        }
    }
}