using Framework;
using UI;
using UI.ViewModel;
using UnityEngine;

namespace Core
{
    public class CommandCenter : MonoBehaviour
    {
        private static int _stationCount;

        public string StationName { get; private set; }

        private TooltipViewModel _tooltipVm;

        void Start()
        {
            StationName = NameGenerator.NumberToName(_stationCount++);
            _tooltipVm = new TooltipViewModel($"Station {StationName}");

        }

        public void OnMouseEnter()
        {
            UiManager.UiManager.Instance.Tooltip.Show(transform, 20, _tooltipVm);
        }

        public void OnMouseExit()
        {
            UiManager.UiManager.Instance.Tooltip.Hide(transform);
        }
    }
}
