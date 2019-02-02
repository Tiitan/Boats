#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct TooltipBaseInfo
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;

        public string Name => _name;
        public string Description => _description;
    }
}