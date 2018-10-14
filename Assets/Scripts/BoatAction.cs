using System;
using UnityEngine;

/// <summary>
/// Event to notify BuilderView to trigger Fx 
/// </summary>
public class BoatActionArg : EventArgs
{
    public Vector3 Position { get; }
    public float Frequency { get; }
    public float Quantity { get; }
    public float Date { get; }

    public BoatActionArg(Vector3 position, float frequency, float date, float quantity)
    {
        Position = position;
        Frequency = frequency;
        Date = date;
        Quantity = quantity;
    }
}

public abstract class BoatAction : MonoBehaviour
{
    [SerializeField] private float _actionFrequency = 0.7f;
    [SerializeField] private int _range = 20;
    [SerializeField] private int _quantity = 1;

    private float _lastExecute;

    public event EventHandler<BoatActionArg> NotifyBoatAction;


    protected int Quantity { get { return _quantity; } }

    /// <summary>Try to continue an action. Called each update</summary>
    /// <returns>1: is over ? 2: did succeed ?</returns>
    public bool TryExecute(MonoBehaviour target, Inventory inventory)
    {
        if (Vector3.Distance(transform.position, target.transform.position) < _range && _actionFrequency <= Time.timeSinceLevelLoad - _lastExecute)
        {
            _lastExecute = Time.timeSinceLevelLoad;
            bool /*(bool over, bool succeed)*/ result = Execute(target, inventory);
            if (result)//.succeed)
                NotifyBoatAction?.Invoke(this, new BoatActionArg(target.transform.position, _actionFrequency, _lastExecute, _quantity));

            // return result.over;
        }
        return false;
    }

    protected abstract bool /*(bool, bool)*/ Execute(MonoBehaviour target, Inventory inventory);

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
