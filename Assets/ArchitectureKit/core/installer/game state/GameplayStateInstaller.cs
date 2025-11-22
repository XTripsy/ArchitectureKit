using UnityEngine.InputSystem;
using Namespace_InputGameplay;
using Namespace_InputGameplay_Event;
using Namespace_Input;
using Namespace_StateGameplay_Event;

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

            state.IRegister(name_state, new GameplayState(bus));

            InputGroup group = installer.IGetGroup<InputGroup>();
            int index = input.IGetIndexCatalogInputAction(name_mapping, group.catalog);
            InputActionMap inputAction = group.action.FindActionMap(name_mapping, throwIfNotFound: false);
            InputCatalog.Mapping mapping = group.catalog.InputAction[index];
            IAction action = new ActionGameplayState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_mapping, action);

            InputActionGameplayState temp = new InputActionGameplayState(bus);
            bus.ISubscribe<ActionClickGameplayState>(_ => temp.ClickGameplay());
            bus.ISubscribe<ActionPauseGameplayState>(_ => temp.PauseGameplay());

            bus.ISubscribe<GameplayStateEnter>(_ => input.IActiveActionInput(name_mapping));
        }
    }
}