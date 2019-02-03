using Framework;
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
            _tooltipVm = new TooltipViewModel(transform, 20, $"Station {StationName}");

        }

        public void OnMouseEnter()
        {
            CoreContainerViewModel.Instance.Tooltip = _tooltipVm;
        }

        public void OnMouseExit()
        {
            if (CoreContainerViewModel.Instance.Tooltip == _tooltipVm)
                CoreContainerViewModel.Instance.Tooltip = null;
        }
    }
}
