using Enums;
using Framework;

public interface IHexaGridItem
{
    HexaType Type { get; }
    CubeCoord Direction { get; }
    CubeCoord CubeCoordLocalPosition { get; }
}
