#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System;
using EventArgs;
using UI;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class BuildManager : MonoBehaviour, IController
    {
        private CommandViewModel _buildCommand;
        private Plane _buildPlane = new Plane(Vector3.up, Vector3.zero);

        private GameObject _cursor;

        private bool _isObstructed;
        private bool _stationProximity;

        [SerializeField] private GameObject _cursorPrefab;
        [SerializeField] private GameObject _blueprintPrefab;

        public event EventHandler<CanBuildAtLocationChangedArg> CanBuildAtLocationChanged;

        private StationsManager _stationManager;
        private ControllerManager _controllerManager;

        private void Start()
        {
            _buildCommand = new CommandViewModel("New station", OnBuildCommand);
            UiManager.Instance.CommandUiView.Register(_buildCommand);
            _stationManager = LevelManager.Instance.StationsManager;
            _controllerManager = LevelManager.Instance.ControllerManager;
        }

        private void OnDestroy()
        {
            if (UiManager.Instance && UiManager.Instance.CommandUiView)
                UiManager.Instance.CommandUiView.UnRegister(_buildCommand);
        }

        private void OnBuildCommand()
        {
            _controllerManager.ClaimControl(this);
        }

        public void ControllerUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && 
                _buildPlane.Raycast(ray, out float distance))
            {
                var raycastPoint = ray.GetPoint(distance);
                bool isObstructed = !NavMesh.SamplePosition(raycastPoint, out var navMeshHit, 5f, NavMesh.AllAreas);
                bool stationProximity = _stationManager.InAnyStationProximity(raycastPoint);

                _cursor.transform.position = isObstructed ? raycastPoint : navMeshHit.position;

                if (!isObstructed && !stationProximity && Input.GetButtonUp("GizmoSubmit"))
                {
                    Instantiate(_blueprintPrefab, _cursor.transform.position, _cursor.transform.rotation);
                    _controllerManager.UnRegister(this);
                }

                if (isObstructed != _isObstructed || _stationProximity != stationProximity)
                {
                    CanBuildAtLocationChanged?.Invoke(this, new CanBuildAtLocationChangedArg(isObstructed, stationProximity));
                    _isObstructed = isObstructed;
                    _stationProximity = stationProximity;
                }
            }

            if (Input.GetButtonUp("GizmoCancel"))
                _controllerManager.UnRegister(this);
        }

        public void GotFocus()
        {
            if (_cursor == null)
                _cursor = Instantiate(_cursorPrefab, transform);
        }

        public void LostFocus()
        {
            if (_cursor != null)
                Destroy(_cursor);
        }
    }
}
