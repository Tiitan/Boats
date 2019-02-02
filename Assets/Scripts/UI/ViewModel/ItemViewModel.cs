using System.ComponentModel;
using ScriptableObjects;

namespace UI.ViewModel
{
    public interface IItemViewModel : INotifyPropertyChanged
    {
        ItemTypeObject Type { get; }

        int Quantity { get; }
    }

    public class ItemViewModel : ObservableObject, IItemViewModel
    {
        private Item _item;

        public ItemTypeObject Type => _item.Type;
        public int Quantity => _item.Quantity;

        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public ItemViewModel(ItemTypeObject type, int quantity)
        {
            _item = new Item(type, quantity);
        }

        public void Drop(int quantity)
        {
            Item = _item.Drop(quantity);
        }

        public void Pick(ref int quantity)
        {
            Item = _item.Pick(ref quantity);
        }
    }
}