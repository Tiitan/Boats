using System.Collections.Generic;
using UI.ViewModel;

namespace Core.UiManager
{
    public interface ICommandUiView
    {
        void Register(ICommandViewModel commandVm, object parent = null);
        void UnRegister(ICommandViewModel commandVm);
        void Register(IEnumerable<ICommandViewModel> commandVms, object parent = null);
        void UnRegister(IEnumerable<ICommandViewModel> commandVms);
    }
}