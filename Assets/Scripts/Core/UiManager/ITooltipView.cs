using UI.ViewModel;
using UnityEngine;

namespace Core.UiManager
{
    public interface ITooltipView
    {
        void Show(Transform target, int pixelRadius, ITooltipViewModel tooltipVm);
        void Hide(Transform target);
    }
}