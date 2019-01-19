using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Framework
{
    /// <summary>
    /// hexagon grid's Cube coordinate system (pointy top)
    /// https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    [Serializable]
    public struct CubeCoord: IEquatable<CubeCoord>
    {
        /// <summary>
        /// Ordered direction, inverse trigo from +Q
        /// </summary>
        public static readonly CubeCoord[] Directions = {
            new CubeCoord(+1, 0),
            new CubeCoord(0, +1),
            new CubeCoord(-1, +1),
            new CubeCoord(-1, 0),
            new CubeCoord(0, -1),
            new CubeCoord(+1, -1)
        };

        /// <summary>
        /// Side of  an hexagon, update to match grid size.
        /// </summary>
        private const float HexaSide = 8f;

        private static readonly float Sqrt3 = Mathf.Sqrt(3f);

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

        public Vector3 ToVector()
        {
            var x = HexaSide * (Sqrt3 * Q + Sqrt3 / 2 * R);
            var z = -HexaSide * (1.5f * R);
            return new Vector3(x, 0, z);
        }

        public float ToAngle() => Vector3.SignedAngle(Vector3.right, ToVector(), Vector3.up);

        public static CubeCoord RoundFromVector(Vector3 localPosition)
        {
            int q = Mathf.RoundToInt((localPosition.x * Sqrt3 / 3 + localPosition.z / 3) / HexaSide);
            int r = Mathf.RoundToInt(-localPosition.z * 2 / 3 / HexaSide);
            return new CubeCoord(q, r);
        }

        public static CubeCoord RoundFromAngle(float angle)
        {
            return Directions[Math.Mod(Mathf.RoundToInt(angle / 60), 6)];
        }

        public static CubeCoord operator +(CubeCoord a, CubeCoord b)
        {
            return new CubeCoord(a.Q + b.Q, a.R + b.R);
        }

        public static CubeCoord operator -(CubeCoord a, CubeCoord b)
        {
            return new CubeCoord(a.Q - b.Q, a.R - b.R);
        }

        public static bool operator ==(CubeCoord a, CubeCoord b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CubeCoord a, CubeCoord b)
        {
            return !(a == b);
        }

        public bool Equals(CubeCoord other)
        {
            return Q == other.Q && R == other.R;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CubeCoord coord && Equals(coord);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Q * 397) ^ R;
            }
        }
    }
}