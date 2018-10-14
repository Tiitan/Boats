#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class InventoryView : MonoBehaviour
    {
        private readonly List<ItemView> _itemsView = new List<ItemView>();
        private IReadOnlyObservableDictionary<ItemType, IItemViewModel> _itemsVm;

        [SerializeField] private RectTransform _itemsTransform;

        [SerializeField] private GameObject _itemPrefab;

        public void Initialize(IReadOnlyObservableDictionary<ItemType, IItemViewModel> itemsVm)
        {
            _itemsVm = itemsVm;
            itemsVm.CollectionChanged += ItemsVmOnCollectionChanged;

            foreach (var itemVm in itemsVm.Values)
                _itemsView.Add(ViewManager.Instantiate<ItemView, IItemViewModel>(itemVm, _itemPrefab, _itemsTransform));
        }

        void OnDestroy()
        {
            _itemsVm.CollectionChanged -= ItemsVmOnCollectionChanged;
        }

        private void ItemsVmOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            // Add
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
                foreach (var key in notifyCollectionChangedEventArgs.NewItems.Cast<ItemType>())
                    _itemsView.Add(ViewManager.Instantiate<ItemView, IItemViewModel>(_itemsVm[key], _itemPrefab, _itemsTransform));

            // Remove
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove)
                foreach (var key in notifyCollectionChangedEventArgs.OldItems.Cast<ItemType>())
                {
                    var view = _itemsView.First(x => x.ItemVm.Type == key);
                    _itemsView.Remove(view);
                    GameObject.Destroy(view.gameObject);
                }
        }
    }
}
