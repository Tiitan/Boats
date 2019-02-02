#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.Collections.Generic;
using Core.UiManager;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class CommandUiView : MonoBehaviour, ICommandUiView
    {
        [SerializeField] private GameObject _commandPrefab;

        private readonly Dictionary<ICommandViewModel, CommandView> _commands = 
            new Dictionary<ICommandViewModel, CommandView>();

        void Awake()
        {
            UiManager.Instance.CommandUi = this;
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
