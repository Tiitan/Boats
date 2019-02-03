using Models.ScriptableObjects;
using UI.ViewModel;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Inventory))]
    public class InventoryTransfer : MonoBehaviour, IInventoryTransfer
    {
        private Selector _selector;
        private CommandViewModel _transferCommand;
        private Inventory _localInventory;
        private Inventory _distantInventory;

        public IInventory LocalInventory => _localInventory;
        public IInventory DistantInventory => _distantInventory;

        private void Start()
        {
            _localInventory = GetComponent<Inventory>();
            _selector = GetComponentInChildren<Selector>();
            _transferCommand = new CommandViewModel("Transfer", OnTransferCommand);
            CoreContainerViewModel.Instance.CommandUi.Register(_transferCommand);
        }

        private void OnTransferCommand()
        {
            _distantInventory = null;
            foreach (var selectedTarget in _selector.SelectedTargets)
            {
                _distantInventory = selectedTarget.GetComponent<Inventory>();
                if (_distantInventory != null)
                    break;
            }
            CoreContainerViewModel.Instance.OpenTransferPanel(this);
        }

        public void Receive(ItemTypeObject itemType, int quantity)
        {
            quantity = _distantInventory.Pick(itemType, quantity);
            _localInventory.Add(itemType, quantity);
        }

        public void Send(ItemTypeObject itemType, int quantity)
        {
            quantity = _localInventory.Pick(itemType, quantity);
            _distantInventory.Add(itemType, quantity);
        }

        public void JettisonLocal(ItemTypeObject itemType, int quantity)
        {
            _localInventory.Pick(itemType, quantity);
        }

        public void JettisonDistant(ItemTypeObject itemType, int quantity)
        {
            _distantInventory.Pick(itemType, quantity);
        }
    }
}
