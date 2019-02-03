#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private Text _textTitle;
        [SerializeField] private Text _textDescription;
        [SerializeField] private RectTransform _propertiesTransform;
        [SerializeField] private GameObject _propertyPrefab;

        private readonly List<TooltipPropertyView> _propertiesViews = new List<TooltipPropertyView>();
        private ITooltipViewModel _tooltipVm;

        private RectTransform RectTransform => (RectTransform)transform;

        void Start()
        {
            CoreContainerViewModel.Instance.PropertyChanged += UiManagerPropertyChanged;
            gameObject.SetActive(false);
        }

        private void UiManagerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(CoreContainerViewModel.Instance.Tooltip)) return;
            
            if (_tooltipVm != null)
                Hide();
            if (CoreContainerViewModel.Instance.Tooltip != null)
                Show(CoreContainerViewModel.Instance.Tooltip);
        }

        private void Show(ITooltipViewModel tooltipVm)
        {
            _textTitle.text = tooltipVm.Name;
            _textDescription.text = tooltipVm.Description;

            _tooltipVm = tooltipVm;
            _tooltipVm.Properties.CollectionChanged += PropertiesOnCollectionChanged;

            foreach (var propertyVm in tooltipVm.Properties.Values)
                _propertiesViews.Add(ViewManager.Instantiate<TooltipPropertyView, ITooltipPropertyViewModel>(
                    propertyVm, _propertyPrefab, _propertiesTransform));

            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases(); // force to recompute the ContentSizeFitter immediatly for correct height.
            var position = Camera.main.WorldToScreenPoint(tooltipVm.Target.position);
            transform.position = new Vector3(position.x + RectTransform.rect.width / 2 + tooltipVm.PixelRadius, 
                                             position.y + RectTransform.rect.height / 2 + tooltipVm.PixelRadius, 
                                             position.z);
        }

        void OnDestroy()
        {
            if (CoreContainerViewModel.Instance != null)
                CoreContainerViewModel.Instance.PropertyChanged -= UiManagerPropertyChanged;
        }

        private void Hide()
        {
            foreach (var propertyView in _propertiesViews)
                Destroy(propertyView.gameObject);
            _propertiesViews.Clear();

            _tooltipVm.Properties.CollectionChanged -= PropertiesOnCollectionChanged;
            _tooltipVm = null;

            gameObject.SetActive(false);
        }

        private void PropertiesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            // TODO: update _propertiesViews
        }
    }
}
