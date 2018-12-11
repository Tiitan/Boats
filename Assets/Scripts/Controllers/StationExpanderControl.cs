#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    class StationExpanderControl : MonoBehaviour, IController
    {
        [SerializeField] private GameObject _expanderGizmoPrefab;

        private ControllerManager _controllerManager;
        private Station _station;
        private Vector3[] _expansionLocations;
        private readonly List<GameObject> _expanderGizmos = new List<GameObject>();

        void Start()
        {
            _controllerManager = LevelManager.Instance.ControllerManager;
            _station = GetComponentInParent<Station>();
        }

        public void ControllerUpdate()
        {
            if (Input.GetButtonUp("GizmoCancel"))
                _controllerManager.UnRegister(this);
        }

        public void GotFocus()
        {
            _expansionLocations = _station.GetExtansionLocation();
            foreach (var locaion in _expansionLocations)
                _expanderGizmos.Add(Instantiate(_expanderGizmoPrefab, transform.position + locaion, Quaternion.identity, transform));
        }

        public void LostFocus()
        {
            foreach (var expanderGizmo in _expanderGizmos)
                Destroy(expanderGizmo);
            _expanderGizmos.Clear();
        }
    }
}
