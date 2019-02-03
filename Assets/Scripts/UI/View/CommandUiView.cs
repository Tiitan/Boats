#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class CommandUiView : MonoBehaviour
    {
        private ICommandUiViewModel _commandUiVm;

        [SerializeField] private GameObject _commandPrefab;

        private readonly Dictionary<ICommandViewModel, CommandView> _commands = 
            new Dictionary<ICommandViewModel, CommandView>();

        private bool _isInitialized;

        void Start()
        {
            if (_isInitialized) return; // Subcommand

            if (CoreContainerViewModel.Instance.CommandUi != null)
                Initialize(CoreContainerViewModel.Instance.CommandUi);
            else
                CoreContainerViewModel.Instance.PropertyChanged += UiManagerPropertyChanged;
        }

        private void UiManagerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(CoreContainerViewModel.Instance.CommandUi)) return;

            Initialize(CoreContainerViewModel.Instance.CommandUi);
            CoreContainerViewModel.Instance.PropertyChanged -= UiManagerPropertyChanged;
        }

        void OnDestroy()
        {
            if (_commandUiVm != null)
                _commandUiVm.Commands.CollectionChanged -= CommandsOnCollectionChanged;
        }

        public void Initialize(ICommandUiViewModel commandUiVm)
        {
            _isInitialized = true;
            if (commandUiVm != null) // subcommand doesn't have ICommandUiViewModel
            {
                _commandUiVm = commandUiVm;
                _commandUiVm.Commands.CollectionChanged += CommandsOnCollectionChanged;
                Register(_commandUiVm.Commands);
            }
        }

        private void CommandsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
                Register(notifyCollectionChangedEventArgs.NewItems.Cast<ICommandViewModel>());
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove)
                UnRegister(notifyCollectionChangedEventArgs.OldItems.Cast<ICommandViewModel>());
        }


        public void Register(ICommandViewModel commandVm, object parent = null)
        {
            if (_commands.ContainsKey(commandVm)) return;
            var commandView = ViewManager.Instantiate<CommandView, ICommandViewModel>(
                commandVm, _commandPrefab, transform);
            _commands.Add(commandVm, commandView);

            // CommandView parent in case of subcommand
            var parentCommend = parent as CommandView; // cast to avoid adding CommandView to external interface
            if (parentCommend != null)
                commandView.Button.onClick.AddListener(parentCommend.SubCommandPressed);
        }

        public void UnRegister(ICommandViewModel commandVm)
        {
            if (!_commands.ContainsKey(commandVm)) return;
            if (_commands[commandVm] != null)
                Destroy(_commands[commandVm].gameObject);
            _commands.Remove(commandVm);
        }

        public void Register(IEnumerable<ICommandViewModel> commandVms, object parent = null)
        {
            foreach (var commandVm in commandVms)
                Register(commandVm, parent);
        }

        public void UnRegister(IEnumerable<ICommandViewModel> commandVms)
        {
            foreach (var commandVm in commandVms)
                UnRegister(commandVm);
        }
    }
}
