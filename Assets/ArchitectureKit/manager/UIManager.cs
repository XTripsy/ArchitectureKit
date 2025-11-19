using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Manager, IUI
{
    /*public Canvas _canvas;
    public UICatalog _catalog;*/
    private UIGroup _group;
    private IFactory<FactoryComponent.Args, GameObject> _factory;

    private Dictionary<string, UICatalog.Entry> _prefabs = new();
    private Dictionary<string, GameObject> _inst = new();
    private Dictionary<(string, string), Component> _cache = new();

    public UIManager(UIGroup group, IFactory<FactoryComponent.Args, GameObject> factory)
    {
        _group = group;
        _factory = factory;

        _prefabs.Clear();
        if (_group.catalog == null || _group.canvas == null) return;
        foreach (var e in _group.catalog.entries)
            if (!string.IsNullOrWhiteSpace(e.name) && e.prefab != null)
                _prefabs[e.name] = e;
    }

    public void IShow(string name) 
    { 
        _Ensure(name)?.SetActive(true);
    }
    
    public void IHide(string name) 
    { 
        if (_inst.TryGetValue(name, out var go) && go) go.SetActive(false); 
    }

    public GameObject IGet(string name) => _Ensure(name);

    public T IGetComponentInUI<T>(string name, string path) where T : Component
    {
        var key = (name, path); 
        if (_cache.TryGetValue(key, out var c) && c) return (T)c;

        var root = _Ensure(name); 
        if (!root) return null; 

        var tr = _FindChild(root.transform, path); 
        if (!tr) return null;

        var comp = tr.GetComponent<T>(); 
        _cache[key] = comp; 
        return comp;
    }

    private GameObject _Ensure(string name)
    {
        if (_inst.TryGetValue(name, out var existing) && existing) return existing;
        if (!_prefabs.TryGetValue(name, out var entry) || !entry.prefab) return null;

        FactoryComponent.Args temp = new FactoryComponent.Args(entry.prefab, _group.canvas.transform, FactoryComponent.EType.eRectTransform);
        existing = _factory.Create(temp);
        _inst[name] = existing;
        _ApplyCanvasOptions(existing, name);
        if (entry.defaultHidden) existing.SetActive(false);
        return existing;
    }

    private void _ApplyCanvasOptions(GameObject go, string name)
    {
        if (!_prefabs.TryGetValue(name, out var e)) return;

        var c = go.GetComponent<Canvas>();
        if (c && e.overrideSorting)
        {
            c.overrideSorting = true;
            c.sortingOrder = e.sortingOrder;
        }
    }

    private static Transform _FindChild(Transform root, string path)
    {
        var cur = root;
        foreach (var p in path.Split('/')) 
        { 
            var f = cur.Find(p); if (!f) return null; cur = f; 
        }
        return cur;
    }

    public void IStart() {}

    public void IUpdate() {}
}
