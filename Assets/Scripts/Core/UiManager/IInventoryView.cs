using Framework.Mvvm;
using Models.ScriptableObjects;
using UI.ViewModel;

namespace Core.UiManager
{
    public interface IInventoryView
    {
        void Initialize(IReadOnlyObservableDictionary<ItemTypeObject, IItemViewModel> itemsVm);
    }
}