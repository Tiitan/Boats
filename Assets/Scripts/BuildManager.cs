#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System;
using Enums;
using EventArgs;
using UI;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour
{
    private CommandViewModel _buildCommand;
    private Plane _buildPlane = new Plane(Vector3.up, Vector3.zero);

    private GameObject _cursor;

    private bool _canBuildAtLocation;

    [SerializeField] private GameObject _cursorPrefab;
    [SerializeField] private GameObject _blueprintPrefab;

    public event EventHandler<CanBuildAtLocationChangedArg> CanBuildAtLocationChanged;

    private void ToogleBuildManager(bool e)
    {
        LevelManager.Instance.Control.enabled = !e;
        enabled = e;
        if (e)
            _cursor = Instantiate(_cursorPrefab, transform);
        else if (_cursor)
            Destroy(_cursor);
    }

    private void Start()
    {
        _buildCommand = new CommandViewModel("Build platform", OnBuildCommand);
        UiManager.Instance.CommandUiView.Register(_buildCommand);
        enabled = false;
    }

    private void OnDestroy()
    {
        if (UiManager.Instance && UiManager.Instance.CommandUiView)
            UiManager.Instance.CommandUiView.UnRegister(_buildCommand);
    }

    private void OnBuildCommand()
    {
        ToogleBuildManager(true);        
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_buildPlane.Raycast(ray, out float distance))
        {
            var raycastPoint = ray.GetPoint(distance);
            bool canBuildAtLocation = NavMesh.SamplePosition(raycastPoint, out var navMeshHit, 1f, NavMesh.AllAreas);
            if (canBuildAtLocation)
            {
                _cursor.transform.position = navMeshHit.position;
                if (Input.GetButton("Mouse1"))
                {
                    Instantiate(_blueprintPrefab, _cursor.transform.position, _cursor.transform.rotation);
                    ToogleBuildManager(false);
                }
            }
            else
                _cursor.transform.position = raycastPoint;

            if (canBuildAtLocation != _canBuildAtLocation)
            {
                CanBuildAtLocationChanged?.Invoke(this, new CanBuildAtLocationChangedArg(canBuildAtLocation));
                _canBuildAtLocation = canBuildAtLocation;
            }
        }
        if (Input.GetButton("Mouse2"))
        {
            ToogleBuildManager(false);
        }
    }
}
