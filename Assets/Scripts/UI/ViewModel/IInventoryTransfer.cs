using Models.ScriptableObjects;

namespace UI.ViewModel
{
    public interface IInventoryTransfer
    {
        IInventory LocalInventory { get; }
        IInventory DistantInventory { get; }
        void Receive(ItemTypeObject itemType, int quantity);
        void Send(ItemTypeObject itemType, int quantity);
        void JettisonLocal(ItemTypeObject itemType, int quantity);
        void JettisonDistant(ItemTypeObject itemType, int quantity);
    }
}