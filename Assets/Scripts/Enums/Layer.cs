using System;

namespace Enums
{
    /// <summary>
    /// Match Unity LayerMask configuration.
    /// Avoid magic strings.
    /// </summary>
    [Flags]
    public enum Layer
    {
        None= 0,
        Default = 1,
        TransparentFx = 1 << 1,
        IgnoreRaycast = 1 << 2,
        L3 = 1 << 3,
        Water = 1 << 4,
        Ui = 1 << 5,
        L6 = 1 << 6,
        L7 = 1 << 7,
        Boat = 1 << 8,
        Flying = 1 << 9,
        Island = 1 << 10,
        Ressource = 1 << 11,
        Structure = 1 << 12,
        Blueprint = 1 << 13
    }
}