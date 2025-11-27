using System.Collections.Generic;
using UnityEngine;

namespace Namespace_Object
{
    internal sealed class ObjectManager : IObjectManager
    {
        private ObjectGroup _group;
        private IFactory<FactoryComponent.Args, GameObject> _factory;

        private Dictionary<string, ObjectCatalog.Entry> _prefabs = new();
        private Dictionary<string, GameObject> _inst = new();
        private Dictionary<(string, string), Component> _cache = new();

        public ObjectManager(ObjectGroup group, IFactory<FactoryComponent.Args, GameObject> factory)
        {
            _group = group;
            _factory = factory;

            _prefabs.Clear();
            if (_group.catalog == null || _group.parent == null) return;
            foreach (var e in _group.catalog.entries)
                if (!string.IsNullOrWhiteSpace(e.name) && e.prefab != null)
                    _prefabs[e.name] = e;
        }

        public void IActive(string name)
        {
            _Ensure(name)?.SetActive(true);
        }

        public void IDeActive(string name)
        {
            if (_inst.TryGetValue(name, out var go) && go) go.SetActive(false);
        }

        public GameObject IGet(string name) => _Ensure(name);

        public T IGetComponentInObject<T>(string name, string childPath) where T : Component
        {
            return null;
        }

        private GameObject _Ensure(string name)
        {
            if (_inst.TryGetValue(name, out var existing) && existing) return existing;
            if (!_prefabs.TryGetValue(name, out var entry) || !entry.prefab) return null;

            FactoryComponent.Args temp = new FactoryComponent.Args(entry.prefab, _group.parent, 
                FactoryComponent.EType.eTransform, entry.myTransform.position, entry.myTransform.rotation);
            existing = _factory.Create(temp);
            _inst[name] = existing;
            _ApplyGameObjectOptions(existing, name);
            if (entry.defaultActive) existing.SetActive(false);
            return existing;
        }

        private void _ApplyGameObjectOptions(GameObject go, string name)
        {
            if (!_prefabs.TryGetValue(name, out var e)) return;

            go.layer = LayerMask.NameToLayer(e.layer);
            go.transform.localScale = e.myTransform.scale;
        }
    }
}
