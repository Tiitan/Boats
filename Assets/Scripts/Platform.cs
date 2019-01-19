#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using Enums;
using Framework;
using UnityEngine;

public class Platform : MonoBehaviour, IHexaGridItem
{
    [SerializeField]
    private HexaType _type;

    public HexaType Type => _type;
    public CubeCoord Direction { get; private set; }
    public CubeCoord CubeCoordLocalPosition { get; private set; }

    void Awake()
    {
        CubeCoordLocalPosition = CubeCoord.RoundFromVector(transform.localPosition);
        Direction = CubeCoord.RoundFromAngle(transform.rotation.eulerAngles.y);
    }
}
