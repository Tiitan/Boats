#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using EventArgs;
using UnityEngine;

public class BuildCursor : MonoBehaviour
{
    private BuildManager _buildManager;

    [SerializeField] private Material _material;

    [SerializeField] private Color _canBuildColor;
    [SerializeField] private Color _canNotBuildColor;

    public void Start()
    {
        _buildManager = GetComponentInParent<BuildManager>();
        _buildManager.CanBuildAtLocationChanged += BuildManagerOnCanBuildAtLocationChanged;
        _material.color = _canNotBuildColor;
    }

    public void OnDestroy()
    {
        _buildManager.CanBuildAtLocationChanged -= BuildManagerOnCanBuildAtLocationChanged;
    }

    private void BuildManagerOnCanBuildAtLocationChanged(object sender, CanBuildAtLocationChangedArg canBuildAtLocationChangedArg)
    {
        _material.color = canBuildAtLocationChangedArg.NewState ? _canBuildColor : _canNotBuildColor;
    }
}
