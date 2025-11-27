using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectCatalog", menuName = "ArchitectureKit/ObjectCatalog")]
public class ObjectCatalog : ScriptableObject
{
    [System.Serializable]
    public struct MyTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    [System.Serializable]
    public struct Entry
    {
        public string name;
        public GameObject prefab;
        public MyTransform myTransform;
        public bool defaultActive;
        public string layer;
    }

    public List<Entry> entries = new List<Entry>();
}
