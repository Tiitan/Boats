#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.ViewModel;
using UnityEngine;

[RequireComponent(typeof(Selectable))]
public class Blueprint : MonoBehaviour
{
    [SerializeField] private GameObject _structurePrefab;
    [SerializeField] private float _finalizeDuration = 1.0f;

    [SerializeField] List<Item> _requiredItems = new List<Item>();
    private readonly List<ItemViewModel> _items = new List<ItemViewModel>();

    private bool _buildingFinished;

    private CommandViewModel _cancelCommand;

    public IEnumerable<Item> MissingItems
    {
        get
        {
            return from requiredItem in _requiredItems
                let item = _items.FirstOrDefault(x => x.Type == requiredItem.Type)?.Item ?? default
                where requiredItem.Quantity > item.Quantity
                select new Item(requiredItem.Type, requiredItem.Quantity - item.Quantity);
        }
    }

    public int Build(ItemType type, int quantity)
    {
        if (_buildingFinished)
            return 0;

        var requiredItem = _requiredItems.FirstOrDefault(x => x.Type == type);
        var item = _items.FirstOrDefault(x => x.Type == type);

        if (requiredItem.Quantity <= item?.Quantity || quantity == 0)
            return 0;

        if (item == null)
        {
            item = new ItemViewModel(type, 0);
            _items.Add(item);
        }
        var maxQuantity = requiredItem.Quantity - item.Quantity;
        var usedQuantity = maxQuantity > quantity ? quantity : maxQuantity;
        item.Drop(usedQuantity);
        _tooltipProperties[type.ToString()].Value = GetFormattedQuantity(requiredItem);

        if (!MissingItems.Any())
            StartCoroutine(FinalizeBuilding());

        return usedQuantity;
    }

    private void CancelBlueprintCommand()
    {
        if (_buildingFinished)
            return;
        UiManager.Instance.Tooltip.Hide(transform);
        Destroy(gameObject);
    }

    public IEnumerator FinalizeBuilding()
    {
        _buildingFinished = true;
        if (_structurePrefab != null)
            Instantiate(_structurePrefab, transform.position, transform.rotation, transform.parent);
        LevelManager.Instance.NavMeshSurface.BuildNavMesh();
        UiManager.Instance.Tooltip.Hide(transform);
        GetComponent<Selectable>().DisableSelection();
        yield return new WaitForSeconds(_finalizeDuration);
        Destroy(gameObject);
    }

    private readonly Dictionary<string, TooltipPropertyViewModel> _tooltipProperties = 
        new Dictionary<string, TooltipPropertyViewModel>();

    string GetFormattedQuantity(Item requiredItem)
    {
        return $"{_items.FirstOrDefault(x => requiredItem.Type == x.Type)?.Quantity ?? 0} / {requiredItem.Quantity}";
    }

    private void Start()
    {
        // Tooltip
        foreach (var requiredItem in _requiredItems)
        {
            _tooltipProperties.Add(requiredItem.Type.ToString(), 
                new TooltipPropertyViewModel(requiredItem.Type.ToString(), GetFormattedQuantity(requiredItem)));
        }

        // Command
        _cancelCommand = new CommandViewModel("Cancel build", CancelBlueprintCommand);
        GetComponent<Selectable>().Commands = new[] { _cancelCommand };
    }

    private void OnMouseEnter()
    {
        if (!_buildingFinished)
            UiManager.Instance.Tooltip.Show(transform, 20, "Blueprint", "blueprint description", _tooltipProperties.Values);
    }

    private void OnMouseExit()
    {
        UiManager.Instance.Tooltip.Hide(transform);
    }
}
