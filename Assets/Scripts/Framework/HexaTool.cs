using UnityEngine;

namespace Framework
{
    public static class HexaTool
    {
        /// <summary>
        /// Side of  an hexagon, update to match mesh size.
        /// </summary>
        private const float HexaSide = 8f;

        private static readonly float Sqrt3 = Mathf.Sqrt(3f);
        // private static readonly float Cospi6 = Mathf.Cos(Mathf.PI / 6);
        // private static readonly float HexaHalfWidth = HexaSide;
        // private static readonly float HexaHalfHeight = HexaSide * Cospi6;

        public static  CubeCoord LocalToCubeCoord(Vector3 localPosition)
        {
            int q = Mathf.RoundToInt((localPosition.x * Sqrt3 / 3 - localPosition.z / 3) / HexaSide);
            int r = Mathf.RoundToInt(localPosition.z * 2 / 3 / HexaSide);
            return new CubeCoord(q, r);
        }

        public static Vector3 CubeCoordToLocal(int q, int r)
        {
            var x = HexaSide * (Sqrt3 * q + Sqrt3 / 2 * r);
            var z = HexaSide * (1.5f * r);
            return new Vector3(x, 0, z);
        }
        public static Vector3 CubeCoordToLocal(CubeCoord cubeCoord) => CubeCoordToLocal(cubeCoord.Q, cubeCoord.R);
    }
}
