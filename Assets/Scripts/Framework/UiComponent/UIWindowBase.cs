
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.UiComponent
{
    /// <summary>
    /// Includes a few fixes of my own, mainly to tidy up duplicates, remove unneeded stuff and testing. (nothing major, all the crew above did the hard work!)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Extensions/UI Window Base")]
    public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private int _keepWindowInCanvas = 5;

        private RectTransform _transform;
        private bool _isDragging;
        private Vector3 _originalCoods = Vector3.zero;
        private Canvas _canvas;
        private RectTransform _canvasRectTransform;

        void Start()
        {
            _transform = GetComponent<RectTransform>();
            _originalCoods = _transform.position;
            _canvas = GetComponentInParent<Canvas>();
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        public void CloseWindow()
        {
            Destroy(gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                var delta = ScreenToCanvas(eventData.position) - ScreenToCanvas(eventData.position - eventData.delta);
                _transform.localPosition += delta;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == null)
                return;
            _isDragging = eventData.pointerCurrentRaycast.gameObject.name == name;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
        }

        public void ResetCoordinatePosition()
        {
            _transform.position = _originalCoods;
        }

        private Vector3 ScreenToCanvas(Vector3 screenPosition)
        {
            Vector3 localPosition;
            Vector2 min, max;
            var canvasSize = _canvasRectTransform.sizeDelta;

            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
                _canvas.renderMode == RenderMode.ScreenSpaceCamera && _canvas.worldCamera == null)
            {
                localPosition = screenPosition;

                min = Vector2.zero;
                max = canvasSize;
            }
            else
            {
                var ray = _canvas.worldCamera.ScreenPointToRay(screenPosition);
                var plane = new Plane(_canvasRectTransform.forward, _canvasRectTransform.position);

                if (plane.Raycast(ray, out float distance) == false)
                    throw new Exception("Is it practically possible?");

                var worldPosition = ray.origin + ray.direction * distance;
                localPosition = _canvasRectTransform.InverseTransformPoint(worldPosition);

                min = -Vector2.Scale(canvasSize, _canvasRectTransform.pivot);
                max = Vector2.Scale(canvasSize, Vector2.one - _canvasRectTransform.pivot);
            }

            // keep window inside canvas
            localPosition.x = Mathf.Clamp(localPosition.x, min.x + _keepWindowInCanvas, max.x - _keepWindowInCanvas);
            localPosition.y = Mathf.Clamp(localPosition.y, min.y + _keepWindowInCanvas, max.y - _keepWindowInCanvas);

            return localPosition;
        }
    }
}