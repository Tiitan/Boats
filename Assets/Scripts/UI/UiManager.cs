using UI.View;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {

        public static UiManager Instance { get; private set; }

        public TooltipView Tooltip { get; private set; }

        void Awake()
        {
            Instance = this;
            Tooltip = gameObject.GetComponentInChildren<TooltipView>(true);
        }
    }
}
