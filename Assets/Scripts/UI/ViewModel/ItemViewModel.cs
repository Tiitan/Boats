using System.ComponentModel;

namespace UI.ViewModel
{
    public interface IItemViewModel : INotifyPropertyChanged
    {
        ItemType Type { get; }

        int Quantity { get; }
    }

    public class ItemViewModel : ObservableObject, IItemViewModel
    {
        private Item _item;

        public ItemType Type => _item.Type;
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

        public ItemViewModel(ItemType type, int quantity)
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