using System.Collections.Generic;

namespace UI.ViewModel
{
    public interface ICommandUiViewModel
    {
        IReadOnlyObservableCollection<ICommandViewModel> Commands { get; }

        // TODO: remove dynamic command system
        void Register(CommandViewModel commandVm, object parent = null);
        void UnRegister(CommandViewModel commandVm);
        void Register(IEnumerable<CommandViewModel> commandVms, object parent = null);
        void UnRegister(IEnumerable<CommandViewModel> commandVms);
    }
}