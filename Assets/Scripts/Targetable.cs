#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using UnityEngine;

public enum EntityType
{
    Resource,
    Ship,
    Structure
}

public class Targetable : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectedEffect;

    [SerializeField]
    private EntityType _type;

    public EntityType Type => _type;

    public void Select (bool isSelected)
    {
        if (_selectedEffect != null)
            _selectedEffect.SetActive(isSelected);
    }
}
