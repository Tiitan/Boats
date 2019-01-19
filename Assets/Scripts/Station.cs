#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using Controllers;
using Enums;
using Framework;
using UI;
using UI.ViewModel;
using UnityEngine;

[RequireComponent(typeof(Selectable), typeof(StationExpanderControl))]
public class Station : MonoBehaviour
{
    [SerializeField] GameObject _expansionBlueprintPrefab;
    [SerializeField] GameObject _portExpansionBlueprintPrefab;

    private GameObject _expansionBlueprint;

    private CommandViewModel _expandCommand;
    private CommandViewModel _portExpandCommand;

    private StationExpanderControl _stationExpanderControl;
    private GameObject _selectedBlueprint;

    private StationHexaGrid _hexaGrid;

    private void Start()
    {
        _hexaGrid = new StationHexaGrid(GetComponentInChildren<Platform>());
        LevelManager.Instance.StationsManager.Register(this);

        _stationExpanderControl = GetComponent<StationExpanderControl>();

        // Command
        _expandCommand = new CommandViewModel("Expand", ExpandCommand);
        _portExpandCommand = new CommandViewModel("Port Expand", PortExpandCommand);
        GetComponent<Selectable>().Commands = new[] { _expandCommand, _portExpandCommand};
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance.StationsManager)
            LevelManager.Instance.StationsManager.UnRegister(this);
    }

    private void ExpandCommand()
    {
        _selectedBlueprint = _expansionBlueprintPrefab;
        _stationExpanderControl.Mode = HexaType.Full;
        LevelManager.Instance.ControllerManager.ClaimControl(_stationExpanderControl);
    }

    private void PortExpandCommand()
    {
        _selectedBlueprint = _portExpansionBlueprintPrefab;
        _stationExpanderControl.Mode = HexaType.Port;
        LevelManager.Instance.ControllerManager.ClaimControl(_stationExpanderControl);
    }

    /// <summary>
    /// Find all extansion slot in the station grid then lookup the navmesh to make sur the location is buildable
    /// </summary>
    /// <returns>array of availlable location relative to this transform</returns>
    public List<Vector3> GetExtansionLocation()
    {
        return _hexaGrid.GetExtansionLocations();
        // TODO: check navmesh
    }

    /// <summary>
    /// A position was selected by the stationExpanderControl,
    /// Instantiate an expansion blueprint at that location.
    /// </summary>
    /// <param name="position">Position selected by the stationExpanderControl</param>
    /// <param name="direction">Quaternion direction</param>
    public void SubmitExtansion(Vector3 position, Quaternion direction)
    {
        _expansionBlueprint = Instantiate(_selectedBlueprint, position, direction, transform);
        RefreshExpandCommand();
        // TODO add blueprint to _buildingHexaGrid
    }

    /// <summary>
    /// refresh the expand command status.
    /// TODO: define expand rules
    /// </summary>
    public void RefreshExpandCommand()
    {
        _expandCommand.Enabled = _expansionBlueprint == null;
        _portExpandCommand.Enabled = _expansionBlueprint == null;
    }

    /// <summary>
    /// The blueprint construction finished
    /// </summary>
    /// <param name="expand">new expand</param>
    public void OnExpandFinalized(GameObject expand)
    {
        _hexaGrid.Expand(expand.GetComponent<Platform>());
        _expansionBlueprint = null;
        RefreshExpandCommand();
    }

    /// <summary>
    /// The blueprint construction was canceled
    /// </summary>
    public void OnExpandCanceled()
    {
        _expansionBlueprint = null;
        RefreshExpandCommand();
    }

    public bool CanBuildAtLocation(CubeCoord cubecoord)
    {
        return _hexaGrid[cubecoord] == null;
        // TODO: check navmesh
    }
}
