#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System.Collections.Generic;
using System.Collections.Specialized;
using Core.UiManager;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class TooltipView : MonoBehaviour, ITooltipView
    {
        [SerializeField] private Text _textTitle;
        [SerializeField] private Text _textDescription;
        [SerializeField] private RectTransform _propertiesTransform;
        [SerializeField] private GameObject _propertyPrefab;

        private Transform _target;
        private readonly List<TooltipPropertyView> _propertiesViews = new List<TooltipPropertyView>();
        private ITooltipViewModel _tooltipVm;

        private RectTransform RectTransform => (RectTransform)transform;

        void Awake()
        {
            UiManager.Instance.Tooltip = this;
            gameObject.SetActive(false);
        }

        public void Show(Transform target, int pixelRadius, ITooltipViewModel tooltipVm)
        {
            if (_target != null)
                Hide(_target);

            _textTitle.text = tooltipVm.Name;
            _textDescription.text = tooltipVm.Description;

            _tooltipVm = tooltipVm;
            _tooltipVm.Properties.CollectionChanged += PropertiesOnCollectionChanged;

            foreach (var propertyVm in tooltipVm.Properties.Values)
                _propertiesViews.Add(ViewManager.Instantiate<TooltipPropertyView, ITooltipPropertyViewModel>(
                    propertyVm, _propertyPrefab, _propertiesTransform));

            _target = target;
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases(); // force to recompute the ContentSizeFitter immediatly for correct height.
            var position = Camera.main.WorldToScreenPoint(_target.position);
            transform.position = new Vector3(position.x + RectTransform.rect.width / 2 + pixelRadius, 
                                             position.y + RectTransform.rect.height / 2 + pixelRadius, 
                                             position.z);
        }

        public void Hide(Transform target)
        {
            if (target != _target) return;

            foreach (var propertyView in _propertiesViews)
                GameObject.Destroy(propertyView.gameObject);
            _propertiesViews.Clear();

            _tooltipVm.Properties.CollectionChanged -= PropertiesOnCollectionChanged;
            _tooltipVm = null;

            _target = null;
            gameObject.SetActive(false);
        }

        private void PropertiesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            // TODO: update _propertiesViews
        }
    }
}
