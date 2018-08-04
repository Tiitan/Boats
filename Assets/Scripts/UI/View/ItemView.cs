#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using System.ComponentModel;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class ItemView : MonoBehaviour, IView<IItemViewModel>
    {
        [SerializeField] private Text _textName;

        [SerializeField] private Text _textQuantity;

        public IItemViewModel ItemVm { get; private set; }

        public void Initialize(IItemViewModel itemVm)
        {
            ItemVm = itemVm;
            itemVm.PropertyChanged += PropertyVm_OnPropertyChanged;

            _textName.text = itemVm.Type.ToString();
            _textQuantity.text = itemVm.Quantity.ToString();
        }

        private void OnDestroy()
        {
            ItemVm.PropertyChanged -= PropertyVm_OnPropertyChanged;
        }

        private void PropertyVm_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var itemVm = (IItemViewModel)sender;
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(IItemViewModel.Quantity):
                    _textQuantity.text = itemVm.Quantity.ToString();
                    break;
            }
        }
    }
}
