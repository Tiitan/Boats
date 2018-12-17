using System.Collections.Generic;
using Framework;
using static Framework.HexaTool;
using UnityEngine;

public class StationHexaGrid
{
    // TODO dynamic grid: X * 2 + 1 where X = max hexagon distance
    private readonly CubeCoord _offset = new CubeCoord(8, 8);

    private readonly HexaGrid<GameObject> _grid;

    public StationHexaGrid(GameObject rootPlatform)
    {
        _grid = new HexaGrid<GameObject>(_offset.Q * 2 + 1, _offset.R * 2 + 1)
        {
            [_offset.Q, _offset.R] = rootPlatform
        };
    }

    public void Expand(GameObject expand)
    {
        CubeCoord cubeCoord = LocalToCubeCoord(expand.transform.localPosition);
        // TODO dynamic grid: resize then use position + X
        _grid[_offset.Q + cubeCoord.Q, _offset.R + cubeCoord.R] = expand;
    }

    public List<Vector3> GetExtansionLocations()
    {
        CubeCoord[] neighborsBuffer  = new CubeCoord[6]; 
        List<Vector3> extensionLocations = new List<Vector3>();
        for (int q = 0; q < _grid.GetLength(0); q++)
        for (int r = 0; r < _grid.GetLength(1); r++)
        {
            if (_grid[q, r] != null) continue;
            FillNeighbors(neighborsBuffer, q, r);
            bool foundLink = false, isLockedHexa = false;
            foreach (var neighbor in neighborsBuffer)
            {
                if (_grid[neighbor.Q, neighbor.R] != null)
                {
                    // TODO check platform type and orientation for LockedHexa
                    foundLink = true;
                }
            }
            if (foundLink && !isLockedHexa)
                extensionLocations.Add(CubeCoordToLocal(q - _offset.Q, r - _offset.R));
        }
        return extensionLocations;
    }

    private void FillNeighbors(CubeCoord[] neighborsBuffer, int q, int r)
    {
        neighborsBuffer[0] = new CubeCoord(q + 0, r + 1);
        neighborsBuffer[1] = new CubeCoord(q + 0, r - 1);
        neighborsBuffer[2] = new CubeCoord(q - 1, r + 0);
        neighborsBuffer[3] = new CubeCoord(q + 1, r + 0);
        neighborsBuffer[4] = new CubeCoord(q - 1, r + 1);
        neighborsBuffer[5] = new CubeCoord(q + 1, r - 1);
    }
}
