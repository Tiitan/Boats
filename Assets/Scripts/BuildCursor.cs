#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using EventArgs;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildCursor : MonoBehaviour
{
    private BuildManager _buildManager;

    [SerializeField] private Material _material;

    [SerializeField] private Color _canBuildColor;
    [SerializeField] [FormerlySerializedAs("_canNotBuildColor")] private Color _obstructedColor;
    [SerializeField] private Color _proximityColor;

    public void Start()
    {
        _buildManager = GetComponentInParent<BuildManager>();
        _buildManager.CanBuildAtLocationChanged += BuildManagerOnCanBuildAtLocationChanged;
        _material.color = _canBuildColor;
    }

    public void OnDestroy()
    {
        _buildManager.CanBuildAtLocationChanged -= BuildManagerOnCanBuildAtLocationChanged;
    }

    private void BuildManagerOnCanBuildAtLocationChanged(object sender, CanBuildAtLocationChangedArg canBuildAtLocationChangedArg)
    {
        _material.color = canBuildAtLocationChangedArg.IsObstructed ? 
            _obstructedColor : (canBuildAtLocationChangedArg.IsTooClose ? _proximityColor : _canBuildColor);
    }
}
