#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using Core.Controllers;
using Core.EventArgs;
using UnityEngine;

namespace Visual.Cursors
{
    public class ExpandCursor : MonoBehaviour
    {
        private StationExpanderControl _stationExpanderControl;

        [SerializeField] private Renderer _renderer;

        [SerializeField] private Color _canBuildColor;
        [SerializeField] private Color _invalidPosition;
        [SerializeField] private Color _invalidRotation;

        private Material _material;

        /// <summary>
        /// Intializing in awake instead of start 
        /// because ExpandCursor is instanciated from an update function that need the event registered immediatly
        /// </summary>
        public void Awake()
        {
            _stationExpanderControl = GetComponentInParent<StationExpanderControl>();
            _stationExpanderControl.CanExpandAtLocationChanged += OnCanExpandAtLocationChanged;
            _material = _renderer.material;
            _material.color = _canBuildColor;
        }

        public void OnDestroy()
        {
            _stationExpanderControl.CanExpandAtLocationChanged -= OnCanExpandAtLocationChanged;
            Destroy(_material);
        }

        private void OnCanExpandAtLocationChanged(object sender, CanExpandAtLocationChangedArg canExpandAtLocationChangedArg)
        {
            _material.color = !canExpandAtLocationChangedArg.ValidLocation ?
                _invalidPosition : (canExpandAtLocationChangedArg.ValidOrientation ? _canBuildColor : _invalidRotation);
        }
    }
}
