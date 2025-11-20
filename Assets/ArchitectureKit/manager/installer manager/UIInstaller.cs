using UnityEngine;

namespace MyUI
{
    [System.Serializable]
    internal struct UIGroup
    {
        public Canvas canvas;
        public UICatalog catalog;
    }

    internal class UIInstaller : IUIInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IFactory<FactoryComponent.Args, GameObject> factory_component = new FactoryComponent();
            IEventBus bus = installer.IGetBus;
            UIGroup group = installer.IGetGroup<UIGroup>();

            IUIManager temp = new UIManager(group, factory_component);
            installer.IRegister(temp);

            UIController _uiController = new UIController(bus, temp);
        }
    }
}
