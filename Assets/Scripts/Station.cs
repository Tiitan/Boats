using Framework;
using UI;
using UI.ViewModel;
using UnityEngine;

[RequireComponent(typeof(Selectable))]
public class Station : MonoBehaviour
{
    private static int _stationCount;

    public string StationName { get; private set; }

    private CommandViewModel _expandCommand;

    private void Start()
    {
        LevelManager.Instance.StationsManager.Register(this);

        StationName = NameGenerator.NumberToName(_stationCount++);

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
        
    }

    private void OnMouseEnter()
    {
        UiManager.Instance.Tooltip.Show(transform, 20, $"Station {StationName}");
    }

    private void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }
}
