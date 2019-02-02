using UnityEngine;

namespace Core.UiManager
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance { get; private set; }
        public ITooltipView Tooltip { get; set; }
        public IInventoryView Inventory { get; set; }
        public ICommandUiView CommandUi { get; set; }

        /// <summary>
        /// Script order before default because registering view initialize from awake
        /// </summary>
        void Awake()
        {
            Instance = this;
        }
    }
}
