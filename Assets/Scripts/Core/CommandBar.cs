using System.Collections.Generic;
using UI;
using UI.ViewModel;
using UnityEngine;

namespace Core
{
    public class CommandBar : MonoBehaviour, ICommandUiViewModel
    {
        void Start()
        {
            CoreContainerViewModel.Instance.CommandUi = this;
        }

        readonly ObservableCollection<CommandViewModel> _commands = new ObservableCollection<CommandViewModel>();

        public IReadOnlyObservableCollection<ICommandViewModel> Commands => _commands;

        public void Register(CommandViewModel commandVm, object parent = null)
        {
            _commands.Add(commandVm);
        }

        public void UnRegister(CommandViewModel commandVm)
        {
            _commands.Remove(commandVm);
        }

        public void Register(IEnumerable<CommandViewModel> commandVms, object parent = null)
        {
            foreach (var commandVm in commandVms)
                Register(commandVm, parent);
        }

        public void UnRegister(IEnumerable<CommandViewModel> commandVms)
        {
            foreach (var commandVm in commandVms)
                UnRegister(commandVm);
        }
    }
}
