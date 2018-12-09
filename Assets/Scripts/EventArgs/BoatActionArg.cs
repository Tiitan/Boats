using UnityEngine;

namespace EventArgs
{
    /// <summary>
    /// Used in event that notify BuilderView to trigger Fx 
    /// </summary>
    public class BoatActionArg : System.EventArgs
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
}
