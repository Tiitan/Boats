#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    class StationExpanderControl : MonoBehaviour, IController
    {
        [SerializeField] private GameObject _expanderGizmoPrefab;

        private ControllerManager _controllerManager;
        private Station _station;
        private Vector3[] _expansionLocations;
        private readonly List<GizmoButton> _expanderGizmos = new List<GizmoButton>();

        private GizmoButton _overButton;
        private bool _mouseDown; // prevent glitch when the mouse was not already pressed before controller activation

        void Start()
        {
            _controllerManager = LevelManager.Instance.ControllerManager;
            _station = GetComponentInParent<Station>();
        }

        public void ControllerUpdate()
        {
            if (Input.GetButtonDown("GizmoSubmit"))
                _mouseDown = true;


            if (_mouseDown && Input.GetButtonUp("GizmoSubmit"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, 1000f, (int) Layer.Gizmo))
                {
                    _controllerManager.UnRegister(this);
                }
            }

            if (Input.GetButtonUp("GizmoCancel"))
                _controllerManager.UnRegister(this);
        }

        private void OnGizmoButtonPressed(object sender, System.EventArgs eventArgs)
        {
            var gizmoButton = (GizmoButton)sender;

            _station.SubmitExtansion(gizmoButton.transform.position);
            _controllerManager.UnRegister(this);
        }

        public void GotFocus()
        {
            _mouseDown = false;
            _expansionLocations = _station.GetExtansionLocation();
            foreach (var locaion in _expansionLocations)
            {
                var expanderGizmo = Instantiate(
                    _expanderGizmoPrefab, transform.position + locaion, Quaternion.identity)
                    .GetComponent<GizmoButton>();
                expanderGizmo.GizmoButtonPressed += OnGizmoButtonPressed;
                _expanderGizmos.Add(expanderGizmo);
            }
        }

        public void LostFocus()
        {
            foreach (var expanderGizmo in _expanderGizmos)
            {
                expanderGizmo.GizmoButtonPressed -= OnGizmoButtonPressed;
                Destroy(expanderGizmo.gameObject);
            }
            _expanderGizmos.Clear();
        }
    }
}
