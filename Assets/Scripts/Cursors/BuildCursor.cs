﻿#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using Controllers;
using EventArgs;
using UnityEngine;

namespace Cursors
{
    public class BuildCursor : MonoBehaviour
    {
        private BuildManager _buildManager;

        [SerializeField] private Material _material;

        [SerializeField] private Color _canBuildColor;
        [SerializeField] private Color _obstructedColor;
        [SerializeField] private Color _proximityColor;

        public void Start()
        {
            _buildManager = GetComponentInParent<BuildManager>();
            _buildManager.CanBuildAtLocationChanged += OnCanBuildAtLocationChanged;
            _material.color = _canBuildColor;
        }

        public void OnDestroy()
        {
            _buildManager.CanBuildAtLocationChanged -= OnCanBuildAtLocationChanged;
        }

        private void OnCanBuildAtLocationChanged(object sender, CanBuildAtLocationChangedArg canBuildAtLocationChangedArg)
        {
            _material.color = canBuildAtLocationChangedArg.IsObstructed ? 
                _obstructedColor : (canBuildAtLocationChangedArg.IsTooClose ? _proximityColor : _canBuildColor);
        }
    }
}
