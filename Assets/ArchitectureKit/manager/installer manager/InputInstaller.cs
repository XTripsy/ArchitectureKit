using UnityEngine.InputSystem;

namespace MyInput
{
    [System.Serializable]
    internal struct InputGroup
    {
        public InputActionAsset action;
        public InputCatalog catalog;
    }

    internal sealed class InputInstaller : IInputInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            InputGroup group = installer.IGetGroup<InputGroup>();
            IInputManager temp = new InputManager();
            installer.IRegister(temp);
        }
    }
}