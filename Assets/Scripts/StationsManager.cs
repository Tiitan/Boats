using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationsManager : MonoBehaviour
{
    [SerializeField] private float _stationMinimumProximity = 100;

    readonly List<Station> _stations = new List<Station>();

    public void Register(Station station)
    {
        _stations.Add(station);
    }

    public void UnRegister(Station station)
    {
        _stations.Remove(station);
    }

    public Station GetClosestStation(Vector3 position)
    {
        var closestDistance = float.MaxValue;
        Station closestStation = null;
        foreach (var station in _stations)
        {
            var distance = Vector3.Distance(station.transform.position, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestStation = station;
            }
        }
        return closestStation;
    }

    public bool InAnyStationProximity(Vector3 position)
    {
        return _stations.Any(station => Vector3.Distance(station.transform.position, position) < _stationMinimumProximity);
    }
}
