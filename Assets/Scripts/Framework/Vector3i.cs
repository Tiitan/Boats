using System;

namespace Framework
{
    [Serializable]
    public struct Vector3I
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        /// Alias for hexagonal cube coordinate equations
        public int Q => X;
        public int R => Y;
        public int S => Z;

        public Vector3I(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}