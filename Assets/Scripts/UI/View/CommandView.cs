#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using System.ComponentModel;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.View
{
    public class CommandView : MonoBehaviour, IView<ICommandViewModel>
    {
        private ICommandViewModel _commandVm;

        [SerializeField] private Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _subCommandPrefab;

        private CommandUiView _subCommandUiView;
        private ICommandViewModel _deployedSubCommandParent;

        public Button Button => _button;

        public void Initialize(ICommandViewModel commandVm)
        {
            _commandVm = commandVm;
            _text.text = commandVm.Text;
            _button.interactable = commandVm.Enabled;
            if (commandVm.Action != null)
                _button.onClick.AddListener(new UnityAction(commandVm.Action));
            if (commandVm.SubCommands != null)
                _button.onClick.AddListener(() => ToggleSubCommand(commandVm));
            commandVm.PropertyChanged += CommandVmOnPropertyChanged;
        }

        private void CommandVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ICommandViewModel.Enabled))
                _button.interactable = _commandVm.Enabled;
        }

        private void OnDestroy()
        {
            if (_commandVm != null)
                _commandVm.PropertyChanged -= CommandVmOnPropertyChanged;
        }

        private void ToggleSubCommand(ICommandViewModel commandVm)
        {
            if (_deployedSubCommandParent == null)
            {
                _deployedSubCommandParent = commandVm;
                var subcommandPanel = Instantiate(_subCommandPrefab, transform.position, transform.rotation, transform);
                _subCommandUiView = subcommandPanel.GetComponent<CommandUiView>();
                _subCommandUiView.Register(commandVm.SubCommands, this);
            }
            else
            {
                _deployedSubCommandParent = null;
                Destroy(_subCommandUiView.gameObject);
            }
        }

        public void SubCommandPressed()
        {
            if (_deployedSubCommandParent != null)
            {
                _deployedSubCommandParent = null;
                Destroy(_subCommandUiView.gameObject);
            }
        }
    }
}