#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Models.ScriptableObjects;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _itemsTransform;
        [SerializeField] private GameObject _itemPrefab;

        private IInventory _inventory;
        private readonly List<ItemView> _itemsView = new List<ItemView>();

        void Start()
        {
            if (CoreContainerViewModel.Instance.MainInventory != null)
                Initialize(CoreContainerViewModel.Instance.MainInventory);
            CoreContainerViewModel.Instance.PropertyChanged += UiManagerPropertyChanged;
        }

        private void Initialize(IInventory inventory)
        {
            if (_inventory != null)
            {
                _inventory.Items.CollectionChanged -= ItemsVmOnCollectionChanged;
                foreach (var view in _itemsView)
                    Destroy(view.gameObject);
                _itemsView.Clear();
            }

            _inventory = inventory;
            _inventory.Items.CollectionChanged += ItemsVmOnCollectionChanged;
            foreach (var itemVm in _inventory.Items.Values)
                _itemsView.Add(ViewManager.Instantiate<ItemView, IItemViewModel>(itemVm, _itemPrefab, _itemsTransform));
        }

        private void UiManagerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(CoreContainerViewModel.Instance.MainInventory)) return;

            Initialize(CoreContainerViewModel.Instance.MainInventory);
        }

        void OnDestroy()
        {
            if (_inventory != null)
                _inventory.Items.CollectionChanged -= ItemsVmOnCollectionChanged;
            if (CoreContainerViewModel.Instance != null)
                CoreContainerViewModel.Instance.PropertyChanged -= UiManagerPropertyChanged;
        }

        private void ItemsVmOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            // Add
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
                foreach (var key in notifyCollectionChangedEventArgs.NewItems.Cast<ItemTypeObject>())
                    _itemsView.Add(ViewManager.Instantiate<ItemView, IItemViewModel>(
                        _inventory.Items[key], _itemPrefab, _itemsTransform));

            // Remove
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove)
                foreach (var key in notifyCollectionChangedEventArgs.OldItems.Cast<ItemTypeObject>())
                {
                    var view = _itemsView.First(x => x.ItemVm.Type == key);
                    _itemsView.Remove(view);
                    Destroy(view.gameObject);
                }
        }
    }
}
