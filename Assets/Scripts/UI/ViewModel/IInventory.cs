using Framework.Mvvm;
using Models.ScriptableObjects;

namespace UI.ViewModel
{
    public interface IInventory
    {
        IReadOnlyObservableDictionary<ItemTypeObject, IItemViewModel> Items { get; }
    }
}