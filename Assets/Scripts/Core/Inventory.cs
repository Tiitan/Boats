#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using System.Linq;
using Framework.Mvvm;
using Models.ScriptableObjects;
using UI.ViewModel;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class Inventory
    {
        readonly ObservableDictionary<ItemTypeObject, ItemViewModel> _items = new ObservableDictionary<ItemTypeObject, ItemViewModel>();

        [SerializeField]
        private int _segment;

        [SerializeField]
        private int _segmentSize;

        public IReadOnlyObservableDictionary<ItemTypeObject, IItemViewModel> Items => _items;

        // Max number of different unique resources.
        public int Segment => _segment;

        // Max quantity of each resource. 
        public int SegmentSize => _segmentSize;

        public int UsedSegment => _items.Values.Sum(x => 1 + x.Quantity / _segmentSize);

        public int Add(ItemTypeObject type, int quantity)
        {
            var availableSpace = AvailableSpace(type);
            var dropQuantity = availableSpace < quantity ? availableSpace : quantity;
            if (_items.ContainsKey(type))
                _items[type].Drop(dropQuantity);
            else
                _items.Add(type, new ItemViewModel(type, dropQuantity));
            return quantity - dropQuantity;
        }

        public int Pick(ItemTypeObject type, int quantity)
        {
            if (!_items.ContainsKey(type))
                return 0;

            _items[type].Pick(ref quantity);
            if (_items[type].Quantity == 0)
                _items.Remove(type);
            return quantity;
        }

        public int AvailableSpace(ItemTypeObject type)
        {
            var spaceAvailable = (_segment - UsedSegment) * _segmentSize;
            if (_items.ContainsKey(type))
                spaceAvailable += _segmentSize - _items[type].Quantity % _segmentSize;
            return spaceAvailable;
        }

        public int Quantity(ItemTypeObject type)
        {
            if (_items.ContainsKey(type))
                return _items[type].Quantity;
            return 0;
        }
    }
}
