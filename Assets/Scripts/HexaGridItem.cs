using Enums;
using Framework;
using UnityEngine;

public interface IHexaGridItem
{
    HexaType Type { get; }
    CubeCoord Direction { get; }
    CubeCoord CubeCoordLocalPosition { get; }
}
