using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UICatalog", menuName = "ArchitectureKit/UICatalog")]
public sealed class UICatalog : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public string name;         
        public GameObject prefab;   
        public bool defaultHidden;  
        public bool overrideSorting;
        public int sortingOrder;    
    }

    public List<Entry> entries = new List<Entry>();
}
