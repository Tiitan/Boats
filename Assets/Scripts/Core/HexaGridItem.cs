using Enums;
using Framework;

namespace Core
{
    public interface IHexaGridItem
    {
        HexaType Type { get; }
        CubeCoord Direction { get; }
        CubeCoord CubeCoordLocalPosition { get; }
    }
}
