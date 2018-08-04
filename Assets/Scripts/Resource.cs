using System.Collections.Generic;
using UI;
using UI.ViewModel;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField]
    private Item _resourceItem;

    public ItemType Type => _resourceItem.Type;
    public int Quantity => _resourceItem.Quantity;

    private readonly Dictionary<string, TooltipPropertyViewModel> _tooltipProperties = new Dictionary<string, TooltipPropertyViewModel>
    {
        {nameof(Type), new TooltipPropertyViewModel("Type", string.Empty) },
        {nameof(Quantity), new TooltipPropertyViewModel("Quantity", string.Empty) }
    };

    private void Start()
    {
        _tooltipProperties[nameof(Type)].Value = Type.ToString();
        _tooltipProperties[nameof(Quantity)].Value = Quantity.ToString();
    }

    private void OnMouseEnter()
    {
        UiManager.Instance.Tooltip.Show(transform, 20, Type.ToString(), "resource description", _tooltipProperties.Values);
    }

    private void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }

    public int Harvest(int value)
    {
        _resourceItem = _resourceItem.Pick(ref value);
        _tooltipProperties[nameof(Quantity)].Value = Quantity.ToString();
        return value;
    }
}
