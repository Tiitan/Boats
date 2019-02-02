#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.Collections.Generic;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class CommandUiView : MonoBehaviour
    {
        [SerializeField] private GameObject _commandPrefab;

        private readonly Dictionary<ICommandViewModel, CommandView> _commands = 
            new Dictionary<ICommandViewModel, CommandView>();

        public void Register(ICommandViewModel commandVm, CommandView parent = null)
        {
            if (_commands.ContainsKey(commandVm)) return;
            var commandView = ViewManager.Instantiate<CommandView, ICommandViewModel>(
                commandVm, _commandPrefab, transform);
            _commands.Add(commandVm, commandView);

            // CommandView parent in case of subcommand
            if (parent != null)
                commandView.Button.onClick.AddListener(parent.SubCommandPressed);
        }

        public void UnRegister(ICommandViewModel commandVm)
        {
            if (!_commands.ContainsKey(commandVm)) return;
            if (_commands[commandVm] != null)
                Destroy(_commands[commandVm].gameObject);
            _commands.Remove(commandVm);
        }

        public void Register(IEnumerable<ICommandViewModel> commandVms, CommandView parent = null)
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
