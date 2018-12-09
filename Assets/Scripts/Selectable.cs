#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using Enums;
using UI;
using UI.ViewModel;
using UnityEngine;

/// <summary>
/// All object automatically selected when in player reach.
/// Register commands to the UI.
/// TODO: also used to filter PlayerControl raycast, remove ?
/// </summary>
public class Selectable : MonoBehaviour
{
    [SerializeField] private GameObject _selectedEffect;
    [SerializeField] private EntityType _type;

    private bool _isSelected;

    public EntityType Type => _type;

    public IEnumerable<ICommandViewModel> Commands { get; set; }

    public void Select (bool isSelected)
    {
        _isSelected = isSelected;
        if (_selectedEffect != null)
            _selectedEffect.SetActive(isSelected);

        if (Commands != null)
        {
            if (isSelected)
                UiManager.Instance.CommandUiView.Register(Commands);
            else
                UiManager.Instance.CommandUiView.UnRegister(Commands);
        }
    }

    private void OnDestroy()
    {
        if (_isSelected && Commands != null)
            UiManager.Instance.CommandUiView.UnRegister(Commands);
    }
}