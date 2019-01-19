#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using System.Collections.Generic;
using Enums;
using EventArgs;
using Framework;
using UnityEngine;

namespace Controllers
{
    class StationExpanderControl : MonoBehaviour, IController
    {
        [SerializeField] private GameObject _expanderGizmoPrefab;
        [SerializeField] private GameObject _cursorPrefab;

        private Plane _buildPlane = new Plane(Vector3.up, Vector3.zero);
        private GameObject _cursor;
        private ControllerManager _controllerManager;
        private Station _station;
        private readonly List<GizmoButton> _expanderGizmos = new List<GizmoButton>();
        private int _direction;
        private bool _invalidDirection;
        private Transform _expanderButtonUnderCursor;
        private bool _validRotation;

        private GizmoButton _overButton;
        private bool _mouseDown; // prevent glitch when the mouse was not already pressed before controller activation
        public HexaType Mode { get; set; }

        public event EventHandler<CanExpandAtLocationChangedArg> CanExpandAtLocationChanged;

        void Start()
        {
            _controllerManager = LevelManager.Instance.ControllerManager;
            _station = GetComponentInParent<Station>();
        }

        public void ControllerUpdate()
        {
            Transform expanderButtonUnderCursor = null;
            bool rotationThisFrame = false;
            if (Input.GetButtonDown("GizmoSubmit"))
                _mouseDown = true;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitinfo, 1000f, (int)Layer.Gizmo)) //  Magnet cursor above button
            {
                expanderButtonUnderCursor = hitinfo.transform;
                _cursor.transform.position = hitinfo.transform.position;
            }
            else if (_mouseDown && Input.GetButtonUp("GizmoSubmit")) // Click outside button: leave tool
            {
                _controllerManager.UnRegister(this);
                return;
            }
            else if (_buildPlane.Raycast(ray, out float distance)) // Default: cursor position simple update
            {
                _cursor.transform.position = ray.GetPoint(distance);
            }

            // Apply rotation, FIXME: duplicate with BuildCursor
            if (Mathf.Abs(Input.GetAxis("Rotate")) > Mathf.Epsilon)
            {
                rotationThisFrame = true;
                _direction = _direction + (Input.GetAxis("Rotate") > 0 ? 1 : -1);
                _direction = (_direction + 6) % 6;
                _cursor.transform.rotation = Quaternion.Euler(0, _direction * 60, 0);
            }

            if (expanderButtonUnderCursor != _expanderButtonUnderCursor || rotationThisFrame)
            {
                _expanderButtonUnderCursor = expanderButtonUnderCursor;
                _validRotation = CheckCursorRotation();
                CanExpandAtLocationChanged?.Invoke(this, new CanExpandAtLocationChangedArg(expanderButtonUnderCursor != null, _validRotation));
            }

            if (Input.GetButtonUp("GizmoCancel"))
                _controllerManager.UnRegister(this);
        }

        /// <summary>
        /// Check if the cursor rotation is valid at current position.
        /// A port entry can't be facing a platform
        /// </summary>
        /// <returns>is rotation valid ?</returns>
        private bool CheckCursorRotation()
        {
            if (!_expanderButtonUnderCursor)
                return false;

            if (Mode == HexaType.Port)
            {
                CubeCoord cubecoord = CubeCoord.RoundFromVector(_expanderButtonUnderCursor.localPosition) + CubeCoord.Directions[_direction];
                return _station.CanBuildAtLocation(cubecoord);
            }
            return true;
        }

        private void OnGizmoButtonPressed(object sender, System.EventArgs eventArgs)
        {
            if (!_validRotation)
                return;

            var gizmoButton = (GizmoButton)sender;

            _station.SubmitExtansion(gizmoButton.transform.position, Quaternion.Euler(0, _direction * 60, 0));
            _controllerManager.UnRegister(this);
        }

        public void GotFocus()
        {
            if (_cursor == null)
                _cursor = Instantiate(_cursorPrefab, transform);
            _direction = 0;
            _expanderButtonUnderCursor = transform; // Force a cursor update on first draw

            _mouseDown = false;
            var expansionLocations = _station.GetExtansionLocation();
            foreach (var location in expansionLocations)
            {
                var expanderGizmo = Instantiate(
                    _expanderGizmoPrefab, transform.position + location, Quaternion.identity, transform)
                    .GetComponent<GizmoButton>();
                expanderGizmo.GizmoButtonPressed += OnGizmoButtonPressed;
                _expanderGizmos.Add(expanderGizmo);
            }
        }

        public void LostFocus()
        {
            if (_cursor != null)
                Destroy(_cursor);

            foreach (var expanderGizmo in _expanderGizmos)
            {
                expanderGizmo.GizmoButtonPressed -= OnGizmoButtonPressed;
                Destroy(expanderGizmo.gameObject);
            }
            _expanderGizmos.Clear();
        }
    }
}
