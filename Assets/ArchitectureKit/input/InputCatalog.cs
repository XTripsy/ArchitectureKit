using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputCatalog", menuName = "ArchitectureKit/InputCatalog")]
public class InputCatalog : ScriptableObject
{
    [System.Serializable]
    public struct Mapping
    {
        public string state;
        public string nameMapping;
        public List<string> actions;
    }

    public List<Mapping> InputAction = new List<Mapping>();
}
