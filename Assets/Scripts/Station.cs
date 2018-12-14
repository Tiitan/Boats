#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using Controllers;
using Framework;
using UI;
using UI.ViewModel;
using UnityEngine;

[RequireComponent(typeof(Selectable), typeof(StationExpanderControl))]
public class Station : MonoBehaviour
{
    private const float HexaSideSizeW = 8f;
    private const float HexaSideSizeH = 6.9282f;

    [SerializeField] GameObject _expansionBlueprintPrefab;
    [SerializeField] GameObject _expansionStructurePrefab;

    private GameObject _expansionBlueprint;

    private static int _stationCount;

    public string StationName { get; private set; }

    private CommandViewModel _expandCommand;

    private StationExpanderControl _stationExpanderControl;

    // _buildingHexaGrid

    private void Start()
    {
        LevelManager.Instance.StationsManager.Register(this);

        StationName = NameGenerator.NumberToName(_stationCount++);

        _stationExpanderControl = GetComponent<StationExpanderControl>();
        // Command
        _expandCommand = new CommandViewModel("Expand", ExpandCommand);
        GetComponent<Selectable>().Commands = new[] { _expandCommand };
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance.StationsManager)
            LevelManager.Instance.StationsManager.UnRegister(this);
    }

    private void ExpandCommand()
    {
        LevelManager.Instance.ControllerManager.ClaimControl(_stationExpanderControl);
    }

    private void OnMouseEnter()
    {
        UiManager.Instance.Tooltip.Show(transform, 20, $"Station {StationName}");
    }

    private void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }

    /// <summary>
    /// Find all extansion slot in the station grid then lookup the navmesh to make sur the locaiton is buildable
    /// </summary>
    /// <returns>array of availlable location relative to this transform</returns>
    public Vector3[] GetExtansionLocation()
    {
        // TODO: Station grid lookup placeholder
        return new[] {new Vector3(HexaSideSizeH, 0, HexaSideSizeW * 1.5f), new Vector3(-HexaSideSizeH, 0, -HexaSideSizeW * 1.5f) };
    }

    /// <summary>
    /// A position was selected by the stationExpanderControl,
    /// Instantiate an expansion blueprint at that location.
    /// </summary>
    /// <param name="position">Position selected by the stationExpanderControl</param>
    public void SubmitExtansion(Vector3 position)
    {
        _expansionBlueprint = Instantiate(_expansionBlueprintPrefab, position, Quaternion.identity);
        _expansionBlueprint.GetComponent<Blueprint>().InitializeExpand(this, _expansionStructurePrefab);
        RefreshExpandCommand();
        // TODO add to _buildingHexaGrid
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
}
