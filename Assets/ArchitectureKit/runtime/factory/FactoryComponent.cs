using UnityEngine;

public sealed class FactoryComponent : IFactory<FactoryComponent.Args, GameObject>
{
    public enum EType
    {
        eTransform,
        eRectTransform
    }

    public readonly struct Args
    {
        public readonly GameObject Prefab;
        public readonly Transform Parent;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly EType Type;

        public Args(GameObject prefab, Transform parent = null, EType type = EType.eTransform,
                    Vector3? position = null, Quaternion? rotation = null)
        {
            Prefab = prefab;
            Parent = parent;
            Position = position ?? Vector3.zero;
            Rotation = rotation ?? Quaternion.identity;
            Type = type;
        }
    }

    public GameObject Create(Args a)
    {
        switch(a.Type)
        {
            case EType.eTransform:
                return Object.Instantiate(a.Prefab, a.Position, a.Rotation, a.Parent);
            case EType.eRectTransform:
                return Object.Instantiate(a.Prefab, a.Parent);
        }

        return null;
    }

    public T Create<T>(Args a) where T : Component
    {
        var go = Create(a);
        return go.GetComponent<T>() ?? go.AddComponent<T>();
    }
}