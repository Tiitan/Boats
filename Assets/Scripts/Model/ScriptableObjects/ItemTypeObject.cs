#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedFi

using UnityEngine;

namespace Models.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Custom/ItemType", order = 1)]
    public class ItemTypeObject : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Texture2D _icon;
        [SerializeField] private string _tooltipDescription;

        public string Name => _name;
        public Texture2D Icon => _icon;
        public string Description => _tooltipDescription;
    }
}