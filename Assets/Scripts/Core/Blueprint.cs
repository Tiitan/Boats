#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.BoatActions;
using Models;
using Models.ScriptableObjects;
using UI;
using UI.ViewModel;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Selectable))]
    public class Blueprint : MonoBehaviour, IBoatActionTarget
    {
        [SerializeField] private float _finalizeDuration = 1.0f;

        private StructureTypeObject _structureType;
        private readonly List<ItemViewModel> _items = new List<ItemViewModel>();
        private bool _buildingFinished;
        private CommandViewModel _cancelCommand;
        private TooltipViewModel _tooltipVm;

        public float ActionFrequencyMultiplier => 1f;

        public IEnumerable<Item> MissingItems
        {
            get
            {
                return from requiredItem in _structureType.ConstructionCost
                    let item = _items.FirstOrDefault(x => x.Type == requiredItem.Type)?.Item ?? default
                    where requiredItem.Quantity > item.Quantity
                    select new Item(requiredItem.Type, requiredItem.Quantity - item.Quantity);
            }
        }

        public void Initialize(StructureTypeObject structureType)
        {
            _structureType = structureType;

            // Tooltip
            _tooltipVm = new TooltipViewModel(_structureType.BlueprintTooltip);
            foreach (var requiredItem in _structureType.ConstructionCost)
            {
                _tooltipVm.EditableProperties.Add(requiredItem.Type.Name,
                    new TooltipPropertyViewModel(requiredItem.Type.Name, GetFormattedQuantity(requiredItem)));
            }
        }

        public int Build(ItemTypeObject type, int quantity)
        {
            if (_buildingFinished)
                return 0;

            var requiredItem = _structureType.ConstructionCost.FirstOrDefault(x => x.Type == type);
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
            _tooltipVm.EditableProperties[type.Name].Value = GetFormattedQuantity(requiredItem);

            if (!MissingItems.Any())
                StartCoroutine(FinalizeBuilding());

            return usedQuantity;
        }

        private void CancelBlueprintCommand()
        {
            if (_buildingFinished)
                return;
            GetComponentInParent<Station>()?.OnExpandCanceled();
            UiManager.UiManager.Instance.Tooltip.Hide(transform);
            Destroy(gameObject);
        }

        public IEnumerator FinalizeBuilding()
        {
            _buildingFinished = true;

            var station = GetComponentInParent<Station>();
            var expand = Instantiate(_structureType.StructurePrefab, transform.position,
                transform.rotation, station?.transform);

            station?.OnExpandFinalized(expand);

            LevelManager.Instance.NavMeshSurface.BuildNavMesh();
            UiManager.UiManager.Instance.Tooltip.Hide(transform);
            GetComponent<Selectable>().DisableSelection();
            yield return new WaitForSeconds(_finalizeDuration);
            Destroy(gameObject);
        }

        string GetFormattedQuantity(Item requiredItem)
        {
            return $"{_items.FirstOrDefault(x => requiredItem.Type == x.Type)?.Quantity ?? 0} / {requiredItem.Quantity}";
        }

        private void Start()
        {
            // Command
            _cancelCommand = new CommandViewModel("Cancel build", CancelBlueprintCommand);
            GetComponent<Selectable>().Commands = new[] { _cancelCommand };
        }

        private void OnMouseEnter()
        {
            if (!_buildingFinished)
            {
                UiManager.UiManager.Instance.Tooltip.Show(transform, 20, _tooltipVm);
            }
        }

        private void OnMouseExit()
        {
            UiManager.UiManager.Instance.Tooltip.Hide(transform);
        }
    }
}
