using UnityEngine;

namespace BoatActions
{
    public interface IBoatActionTarget
    {
        float ActionFrequencyMultiplier { get; }

        #pragma warning disable IDE1006 // Styles d'affectation de noms
        // ReSharper disable once InconsistentNaming
        Transform transform { get; }
    }
}
