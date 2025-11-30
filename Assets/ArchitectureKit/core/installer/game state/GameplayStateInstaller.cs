using Namespace_Input;
using Namespace_InputGameplay;
using Namespace_InputGameplay_Event;
using Namespace_StateGameplay_Event;
using UnityEngine.InputSystem;

namespace Namespace_StateGameplay
{
    internal sealed class GameplayStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            string name_state = "gameplay_state";
            string name_mapping = "gameplay_mapping";

            IEventBus bus = installer.IGetBus;
            IStateRegistry state = installer.IGetStateRegistry;
            IInputManager input = installer.IResolve<IInputManager>();
            IGameLoopManager gameLoopManager = installer.IResolve<IGameLoopManager>();

            state.IRegister(name_state, new GameplayState(bus));

            _InstallInput(installer, bus, input, name_mapping);
            _InstallInputAction(bus);
        }

        private void _InstallInput(IBootstrapContext installer, IEventBus bus, IInputManager input, string name_mapping)
        {
            InputGroup group = installer.IGetGroup<InputGroup>();
            int index = input.IGetIndexCatalogInputAction(name_mapping, group.catalog);
            InputActionMap inputAction = group.action.FindActionMap(name_mapping, throwIfNotFound: false);
            InputCatalog.Mapping mapping = group.catalog.InputAction[index];
            IAction action = new ActionGameplayState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_mapping, action);
            bus.ISubscribe<GameplayStateEnter>(_ => input.IActiveActionInput(name_mapping));
        }

        private void _InstallInputAction(IEventBus bus)
        {
            InputActionGameplayState temp = new InputActionGameplayState(bus);
            bus.ISubscribe<ActionClickGameplayState>(_ => temp.ClickGameplay());
            bus.ISubscribe<ActionPauseGameplayState>(_ => temp.PauseGameplay());
        }
    }
}