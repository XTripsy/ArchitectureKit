using Namespace_UI;
using UnityEngine;

namespace Namespace_Object
{
    [System.Serializable]
    internal struct ObjectGroup
    {
        public Transform parent;
        public ObjectCatalog catalog;
    }

    internal sealed class ObjectInstaller : IObjectInstaller
    {
        public void Install(IBootstrapContext installer)
        {
            IFactory<FactoryComponent.Args, GameObject> factory_component = new FactoryComponent();
            IEventBus bus = installer.IGetBus;
            ObjectGroup group = installer.IGetGroup<ObjectGroup>();

            IObjectManager temp = new ObjectManager(group, factory_component);
            installer.IRegister(temp);
        }
    }
}
