using Namespace_Input;
using Namespace_InputPause;
using Namespace_InputPause_Event;
using Namespace_StatePause_Event;
using Namespace_UIPause;
using PurrLobby;
using UnityEngine.InputSystem;

namespace Namespace_StatePause
{
    internal sealed class PauseStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            string name_state = "pause_state";
            string name_mapping = "pause_mapping";

            IEventBus bus = installer.IGetBus;
            IStateRegistry state = installer.IGetStateRegistry;
            IInputManager input = installer.IResolve<IInputManager>();
            IUIManager ui = installer.IResolve<IUIManager>();
            LobbyManager lobbyManager = installer.IResolve<LobbyManager>();

            state.IRegister(name_state, new PauseState(bus));

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
            IAction action = new ActionPauseState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_mapping, action);

            bus.ISubscribe<PauseStateEnter>(_ => input.IActiveActionInput(name_mapping));
        }

        private void _InstallInputAction(IEventBus bus)
        {
            InputActionPauseState temp_input = new InputActionPauseState(bus);
            bus.ISubscribe<ActionResumePauseState>(_ => temp_input.ResumePause());
        }

        private void _InstallUI(IEventBus bus, IUIManager ui, LobbyManager lm)
        {
            UIActionPauseState temp_ui = new UIActionPauseState(bus, ui, lm);
            bus.ISubscribe<PauseStateEnter>(_ => temp_ui.OnPauseEnter());
            bus.ISubscribe<PauseStateExit>(_ => temp_ui.OnPauseExit());
        }
    }
}
