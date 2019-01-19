using Framework;
using UI;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    private static int _stationCount;

    public string StationName { get; private set; }

    void Start()
    {
        StationName = NameGenerator.NumberToName(_stationCount++);
    }

    public void OnMouseEnter()
    {
        UiManager.Instance.Tooltip.Show(transform, 20, $"Station {StationName}");
    }

    public void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }
}
