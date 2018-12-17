using System;
using UnityEngine.Assertions;

namespace Framework
{
    [Serializable]
    public struct CubeCoord
    {
        public int Q { get; }
        public int R { get; }
        public int S { get; }

        public CubeCoord(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
            Assert.AreEqual(q + r + s, 0);
        }
        public CubeCoord(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }
    }
}