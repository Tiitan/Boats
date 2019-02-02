#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using System.Linq;
using Controllers;
using Enums;
using Framework;
using ScriptableObjects;
using UI;
using UI.ViewModel;
using UnityEngine;

[RequireComponent(typeof(Selectable), typeof(StationExpanderControl))]
public class Station : MonoBehaviour
{
    [SerializeField] Texture2D _expandIcon;
    [SerializeField] StructureTypeObjectList _structureTypeList;

    private Blueprint _expansionBlueprint;

    private CommandViewModel _expandCommand;
    private ObservableCollection<CommandViewModel> _subCommands;

    private StationExpanderControl _stationExpanderControl;
    private StructureTypeObject _selectedStructureType;

    private StationHexaGrid _hexaGrid;

    private void Start()
    {
        _hexaGrid = new StationHexaGrid(GetComponentInChildren<Platform>());
        LevelManager.Instance.StationsManager.Register(this);

        _stationExpanderControl = GetComponent<StationExpanderControl>();

        _subCommands = new ObservableCollection<CommandViewModel>(
            _structureTypeList.List.Where(x => x.IsBuildable)
            .Select(x => new CommandViewModel(x.name, () => ExpandCommand(x))));

        // Command
        _expandCommand = new CommandViewModel("Expand", _subCommands);
        GetComponent<Selectable>().Commands = new[] { _expandCommand};
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance.StationsManager)
            LevelManager.Instance.StationsManager.UnRegister(this);
    }

    private void ExpandCommand(StructureTypeObject structureType)
    {
        _selectedStructureType = structureType;
        _stationExpanderControl.Mode = structureType.HexaType;
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
        _expansionBlueprint = Instantiate(_selectedStructureType.BlueprintPrefab, position, direction, transform).GetComponent<Blueprint>();
        _expansionBlueprint.Initialize(_selectedStructureType);
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
