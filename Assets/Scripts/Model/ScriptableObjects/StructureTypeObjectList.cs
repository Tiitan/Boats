#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using UnityEngine;

namespace Models.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StructureTypeList", menuName = "Custom/StructureTypeList", order = 1)]
    public class StructureTypeObjectList : ScriptableObject
    {
        [SerializeField] private StructureTypeObject[] _list;

        public StructureTypeObject[] List => _list;
    }
}
