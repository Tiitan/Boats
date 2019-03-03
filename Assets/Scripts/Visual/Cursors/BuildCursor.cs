#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using Core.Controllers;
using Core.EventArgs;
using UnityEngine;

namespace Visual.Cursors
{
    public class BuildCursor : MonoBehaviour
    {
        private BuildManager _buildManager;

        [SerializeField] private Renderer _renderer;

        [SerializeField] private Color _canBuildColor;
        [SerializeField] private Color _obstructedColor;
        [SerializeField] private Color _proximityColor;

        Material _material;

        public void Start()
        {
            _buildManager = GetComponentInParent<BuildManager>();
            _buildManager.CanBuildAtLocationChanged += OnCanBuildAtLocationChanged;
            _material = _renderer.material;
            _material.color = _canBuildColor;
        }

        public void OnDestroy()
        {
            _buildManager.CanBuildAtLocationChanged -= OnCanBuildAtLocationChanged;
            Destroy(_material);
        }

        private void OnCanBuildAtLocationChanged(object sender, CanBuildAtLocationChangedArg canBuildAtLocationChangedArg)
        {
            _material.color = canBuildAtLocationChangedArg.IsObstructed ? 
                _obstructedColor : (canBuildAtLocationChangedArg.IsTooClose ? _proximityColor : _canBuildColor);
        }
    }
}
