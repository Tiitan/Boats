using System.Collections.Generic;
using UI;
using UI.ViewModel;
using UnityEngine;

public enum ResourceType
{
    R1
}

public class Resource : MonoBehaviour
{
    [SerializeField]
    private ResourceType _type;

    [SerializeField]
    private int _amount;

    public ResourceType Type => _type;
    public int Amount
    {
        get { return _amount; }
        private set
        {
            _amount = value;
            _tooltipProperties[nameof(Amount)].Value = Amount.ToString();
        }
    }

    private readonly Dictionary<string, TooltipPropertyViewModel> _tooltipProperties = new Dictionary<string, TooltipPropertyViewModel>
    {
        {nameof(Type), new TooltipPropertyViewModel("Type", string.Empty) },
        {nameof(Amount), new TooltipPropertyViewModel("Amount", string.Empty) }
    };

    private void Start()
    {
        _tooltipProperties[nameof(Type)].Value = Type.ToString();
        _tooltipProperties[nameof(Amount)].Value = Amount.ToString();
    }

    private void OnMouseEnter()
    {
        UiManager.Instance.Tooltip.Show(transform, 20, _type.ToString(), "resource description", _tooltipProperties.Values);
    }

    private void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }

    public int Harvest(int value)
    {
        if (_amount < value)
            value = _amount;
        Amount -= value;
        return value;
    }
}
