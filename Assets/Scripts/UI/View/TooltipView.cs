using System.Collections;
using System.Collections.Generic;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class TooltipView : MonoBehaviour
    {
        private Transform _target;

        private readonly List<TooltipPropertyView> _propertyViews = new List<TooltipPropertyView>();

        [SerializeField] private Text _textTitle;

        [SerializeField] private Text _textDescription;

        [SerializeField] private RectTransform _propertiesTransform;

        [SerializeField] private GameObject _propertyPrefab;

        private RectTransform RectTransform => (RectTransform)transform;

        public void Show(Transform target, int pixelRadius, string title = null, string description = null, IEnumerable<ITooltipPropertyViewModel> propertyInfos = null)
        {
            if (_target != null)
                Hide(_target);

            _textTitle.text = title;
            _textDescription.text = description;

            if (propertyInfos != null)
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyGameObject = GameObject.Instantiate(_propertyPrefab, _propertiesTransform);
                    var propertyView = propertyGameObject.GetComponent<TooltipPropertyView>();
                    _propertyViews.Add(propertyView);
                    propertyView.Initialize(propertyInfo);
                }

            _target = target;
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases(); // force to recompute the ContentSizeFitter immediatly for correct height.
            var position = Camera.main.WorldToScreenPoint(_target.position);
            transform.position = new Vector3(position.x + RectTransform.rect.width / 2 + pixelRadius, position.y + RectTransform.rect.height / 2 + pixelRadius, position.z);
        }

        public void Hide(Transform target)
        {
            if (target != _target) return;

            foreach (var propertyView in _propertyViews)
                GameObject.Destroy(propertyView.gameObject);
            _propertyViews.Clear();

            _target = null;
            gameObject.SetActive(false);
        }
    }
}
