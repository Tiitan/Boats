#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Models.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Structure", menuName = "Custom/StructureType", order = 1)]
    public class StructureTypeObject : ScriptableObject
    {

        [SerializeField] private string _name;
        [SerializeField] private Texture2D _icon;
        [SerializeField] private TooltipBaseInfo _structureTooltip;
        [SerializeField] private TooltipBaseInfo _blueprintTooltip;
        [SerializeField] private HexaType _hexaType;
        [SerializeField] private GameObject _structurePrefab;
        [SerializeField] private GameObject _blueprintPrefab;
        [SerializeField] private List<Item> _constructionCost;
        [SerializeField] private bool _isLocked;
        [SerializeField] private bool _isBuildable;

        public string Name => _name;
        public Texture2D Icon => _icon;
        public TooltipBaseInfo StructureTooltip => _structureTooltip;
        public TooltipBaseInfo BlueprintTooltip  => _blueprintTooltip;
        public HexaType HexaType => _hexaType;
        public IEnumerable<Item> ConstructionCost => _constructionCost;
        public GameObject StructurePrefab => _structurePrefab;
        public GameObject BlueprintPrefab => _blueprintPrefab;
        public bool IsLocked => _isLocked;
        public bool IsBuildable => _isBuildable;
    }
}