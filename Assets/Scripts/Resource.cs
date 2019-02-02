#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using BoatActions;
using ScriptableObjects;
using UI;
using UI.ViewModel;
using UnityEngine;

public class Resource : MonoBehaviour, IBoatActionTarget
{
    [SerializeField] private ItemTypeObject _type;
    [SerializeField] private int _quantity;
    [SerializeField] private float _harvestFrequencyMultiplier;
    [SerializeField] private float _regenFrequency;

    private float _regenTime;
    private int _maxQuantity;

    public ItemTypeObject Type => _type;
    public int Quantity => _quantity;
    public float ActionFrequencyMultiplier => _harvestFrequencyMultiplier;

    private TooltipViewModel _tooltipVm;

    void Start()
    {
        _maxQuantity = _quantity;
        _tooltipVm = new TooltipViewModel(
            _type.Name, 
            _type.Description,
            new ObservableDictionary<string, TooltipPropertyViewModel>
            {
                {nameof(Type), new TooltipPropertyViewModel("Type", Type.Name) },
                {nameof(Quantity), new TooltipPropertyViewModel("Quantity", Quantity.ToString()) }
            }
        );
    }

    void Update()
    {
        if (_quantity < _maxQuantity && _regenTime + _regenFrequency < Time.timeSinceLevelLoad)
        {
            _quantity += 1;
            _regenTime = Time.timeSinceLevelLoad;
            _tooltipVm.EditableProperties[nameof(Quantity)].Value = Quantity.ToString();
        }
    }

    private void OnMouseEnter()
    {
        UiManager.Instance.Tooltip.Show(transform, 20, _tooltipVm);
    }

    private void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }

    public int Harvest(int value)
    {
        if (_quantity == _maxQuantity)
            _regenTime = Time.time;

        if (_quantity < value)
            value = _quantity;
        _quantity -= value;
        _tooltipVm.EditableProperties[nameof(Quantity)].Value = Quantity.ToString();
        return value;
    }
}
