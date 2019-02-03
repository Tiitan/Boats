#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "add readonly modifier" for SerializedField

using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace UI.ViewModel
{
    /// <summary>
    /// Depency injection container.
    /// Core classes that need to be exposed to the GUI.
    /// </summary>
    public class CoreContainerViewModel : MonoBehaviour, INotifyPropertyChanged
    {
        [SerializeField] private GameObject _transferPanelPrefab;

        private IInventory _mainInventory;
        private ITooltipViewModel _tooltip;
        private ICommandUiViewModel _commandUiVm;
        private GameObject _transferPanel;

        public static CoreContainerViewModel Instance { get; private set; }

        public ICommandUiViewModel CommandUi
        {
            get => _commandUiVm;
            set
            {
                if (_commandUiVm == value) return;
                _commandUiVm = value;
                OnPropertyChanged();
            }
        }

        public ITooltipViewModel Tooltip
        {
            get => _tooltip;
            set
            {
                if (_tooltip == value) return;
                _tooltip = value;
                OnPropertyChanged();
            }
        }

        public IInventory MainInventory
        {
            get => _mainInventory;
            set
            {
                if (_mainInventory == value) return;
                _mainInventory = value;
                OnPropertyChanged();
            }
        }

        public IInventoryTransfer InventoryTransfer { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Script order before default because registering view initialize from awake
        /// </summary>
        void Awake()
        {
            Instance = this;
        }

        public void OpenTransferPanel(IInventoryTransfer transfer)
        {
            InventoryTransfer = transfer;
            if (_transferPanel == null)
                _transferPanel = Instantiate(_transferPanelPrefab, transform);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
