using System;

namespace Framework
{
    [Serializable]
    public struct Vector2I
    {
        public int X { get; }
        public int Y { get; }

        public Vector2I(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}