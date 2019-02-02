#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using Core;
using Core.UiManager;
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
    private bool _isSelectionDisabled;

    public EntityType Type => _type;

    public IEnumerable<ICommandViewModel> Commands { get; set; }

    public void Select (bool isSelected)
    {
        if (_isSelectionDisabled) return;

        if (_selectedEffect != null)
            _selectedEffect.SetActive(isSelected);

        if (Commands != null && _isSelected != isSelected)
        {
            if (isSelected)
                UiManager.Instance.CommandUi.Register(Commands);
            else
                UiManager.Instance.CommandUi.UnRegister(Commands);
        }
        _isSelected = isSelected;
    }

    private void OnDestroy()
    {
        if (_isSelected && Commands != null)
            UiManager.Instance.CommandUi.UnRegister(Commands);
    }

    /// <summary>
    /// Disable selection when this object is about to be destroyed.
    /// </summary>
    public void DisableSelection()
    {
        Select(false);
        _isSelectionDisabled = true;
    }
}