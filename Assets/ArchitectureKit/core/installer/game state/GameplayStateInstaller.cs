using UnityEngine.InputSystem;
using Namespace_InputGameplay;
using Namespace_InputGameplayEvent;
using Namespace_Input;

namespace Namespace_StateGameplay
{
    internal sealed class GameplayStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            string name_state = "gameplay_state";
            IEventBus bus = installer.IGetBus;
            IStateRegistry state = installer.IGetStateRegistry;
            IInputManager input = installer.IResolve<IInputManager>();

            state.IRegister(name_state, new GameplayState(bus));

            InputGroup group = installer.IGetGroup<InputGroup>();
            int index = input.IGetIndexCatalogInputAction("gameplay_mapping", group.catalog);
            InputActionMap inputAction = group.action.FindActionMap("gameplay_mapping", throwIfNotFound: false);
            InputCatalog.Mapping mapping = group.catalog.InputAction[index];
            IAction action = new ActionGameplayState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_state, action);

            InputActionGameplayState temp = new InputActionGameplayState(bus);
            bus.ISubscribe<ActionClickGameplayState>(_ => temp.ClickGameplay());
        }
    }
}