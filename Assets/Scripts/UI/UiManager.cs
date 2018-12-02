using UI.View;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {

        public static UiManager Instance { get; private set; }

        public TooltipView Tooltip { get; private set; }
        public InventoryView Inventory { get; private set; }
        public CommandUiView CommandUiView { get; private set; }

        void Awake()
        {
            Instance = this;
            Tooltip = gameObject.GetComponentInChildren<TooltipView>(true);
            Inventory = gameObject.GetComponentInChildren<InventoryView>(true);
            CommandUiView = gameObject.GetComponentInChildren<CommandUiView>(true);
        }
    }
}
