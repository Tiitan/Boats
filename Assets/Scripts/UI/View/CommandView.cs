#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using System.ComponentModel;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.View
{
    public class CommandView : MonoBehaviour
    {
        private ICommandViewModel _commandVm;

        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        public void Initialize(ICommandViewModel commandVm)
        {
            _commandVm = commandVm;
            _text.text = commandVm.Text;
            _button.interactable = commandVm.Enabled;
            _button.onClick.AddListener(new UnityAction(commandVm.Action));
            commandVm.PropertyChanged += CommandVmOnPropertyChanged;
        }

        private void CommandVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ICommandViewModel.Enabled))
                _button.interactable = _commandVm.Enabled;
        }

        private void OnDestroy()
        {
            if (_commandVm != null)
                _commandVm.PropertyChanged -= CommandVmOnPropertyChanged;
        }
    }
}
