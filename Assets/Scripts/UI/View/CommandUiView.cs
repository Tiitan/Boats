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

        readonly Dictionary<ICommandViewModel, GameObject> _commands = new Dictionary<ICommandViewModel, GameObject>();

        public void Register(ICommandViewModel commandVm)
        {
            var commandObject = Instantiate(_commandPrefab, transform.position, transform.rotation, transform);
            var commmandView = commandObject.GetComponent<CommandView>();
            commmandView.Initialize(commandVm);
            _commands.Add(commandVm, commandObject);
        }

        public void UnRegister(ICommandViewModel commandVm)
        {
            if (!_commands.ContainsKey(commandVm)) return;
            if (_commands[commandVm] != null)
                Destroy(_commands[commandVm]);
            _commands.Remove(commandVm);
        }

        public void Register(IEnumerable<ICommandViewModel> commandVms)
        {
            foreach (var commandVm in commandVms)
                Register(commandVm);
        }

        public void UnRegister(IEnumerable<ICommandViewModel> commandVms)
        {
            foreach (var commandVm in commandVms)
                UnRegister(commandVm);
        }
    }
}
