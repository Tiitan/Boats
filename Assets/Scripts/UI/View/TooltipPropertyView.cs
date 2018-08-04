#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using System.ComponentModel;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class TooltipPropertyView : MonoBehaviour, IView<ITooltipPropertyViewModel>
    {
        [SerializeField] private Text _textName;

        [SerializeField] private Text _textValue;

        private ITooltipPropertyViewModel _propertyVm;

        public void Initialize(ITooltipPropertyViewModel propertyVm)
        {
            _propertyVm = propertyVm;
            propertyVm.PropertyChanged += PropertyVm_OnPropertyChanged;

            _textName.text = propertyVm.Name;
            _textValue.text = propertyVm.Value;
        }

        private void OnDestroy()
        {
            _propertyVm.PropertyChanged -= PropertyVm_OnPropertyChanged;
        }

        private void PropertyVm_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var tooltipPropertyViewModel = (ITooltipPropertyViewModel) sender;
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(ITooltipPropertyViewModel.Value):
                    _textValue.text = tooltipPropertyViewModel.Value;
                    break;
            }
        }
    }
}