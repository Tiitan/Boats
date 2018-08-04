#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using UnityEngine;

public class HarvestArg : EventArgs
{
    public Vector3 Position { get; }
    public float Frequency { get; }
    public float Quantity { get; }
    public float Date { get; }

    public HarvestArg(Vector3 position, float frequency, float date, float quantity)
    {
        Position = position;
        Frequency = frequency;
        Date = date;
        Quantity = quantity;
    }
}

public class Harvester : MonoBehaviour
{
    [SerializeField] private int _harvestQuantity = 1;
    [SerializeField] private float _harvestFrequency = 0.7f;
    [SerializeField] private int _range = 20;

    private float _lastHarvest;

    public event EventHandler<HarvestArg> NotifyHarvest;

    /// <summary>Try to continue harvesting. Called each update</summary>
    /// <returns>is harvesting over ? inventory full or resource empty AFTER harvesting</returns>
    public bool Harvest(Resource resource, Inventory inventory)
    {
        if (Vector3.Distance(transform.position, resource.transform.position) < _range && _harvestFrequency <= Time.timeSinceLevelLoad - _lastHarvest)
        {
            _lastHarvest = Time.timeSinceLevelLoad;

            var availableSpace = inventory.AvailableSpace(resource.Type);
            var quantity = resource.Harvest(availableSpace < _harvestQuantity ? availableSpace : _harvestQuantity);
            inventory.Add(resource.Type, quantity);

            NotifyHarvest?.Invoke(this, new HarvestArg(resource.transform.position, _harvestFrequency, _lastHarvest, _harvestQuantity));
            return availableSpace == quantity || resource.Quantity == 0;
        }
        return false;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
