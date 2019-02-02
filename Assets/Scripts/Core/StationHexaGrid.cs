using System.Collections.Generic;
using Enums;
using Framework;
using UnityEngine;

namespace Core
{
    public class StationHexaGrid
    {
        // TODO dynamic grid: X * 2 + 1 where X = max hexagon distance
        private static readonly CubeCoord Offset = new CubeCoord(2, 2);

        private readonly HexaGrid<IHexaGridItem> _grid;

        public IHexaGridItem this[CubeCoord c] => _grid[c + Offset];

        public StationHexaGrid(IHexaGridItem rootPlatform)
        {
            _grid = new HexaGrid<IHexaGridItem>(Offset.Q * 2 + 1, Offset.R * 2 + 1)
            {
                [Offset.Q, Offset.R] = rootPlatform
            };
        }

        public void Expand(IHexaGridItem expand)
        {
            // TODO dynamic grid: resize then use position + X
            _grid[Offset + expand.CubeCoordLocalPosition] = expand;
        }

        public List<Vector3> GetExtansionLocations()
        {
            CubeCoord[] neighborsBuffer  = new CubeCoord[6]; 
            List<Vector3> extensionLocations = new List<Vector3>();
            for (int q = 0; q < _grid.GetLength(0); q++)
            for (int r = 0; r < _grid.GetLength(1); r++)
            {
                var coord = new CubeCoord(q, r);
                if (_grid[coord] != null) continue;
                FillNeighbors(neighborsBuffer, coord);
                bool foundLink = false, isLockedHexa = false;
                foreach (var neighbor in neighborsBuffer)
                {
                    if (_grid[neighbor] == null)
                        continue;

                    if (_grid[neighbor].Type == HexaType.Port &&
                        neighbor + _grid[neighbor].Direction == coord)
                    {
                        isLockedHexa = true;
                        break;
                    }
                    foundLink = true;
                }
                if (foundLink && !isLockedHexa)
                    extensionLocations.Add((coord - Offset).ToVector());
            }
            return extensionLocations;
        }

        private void FillNeighbors(CubeCoord[] neighborsBuffer, CubeCoord coord)
        {
            for (int i = 0; i < 6; i++)
                neighborsBuffer[i] = CubeCoord.Directions[i] + coord;
        }
    }
}
