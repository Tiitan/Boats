#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using System.Collections;
using EventArgs;
using UnityEngine;

namespace Effect
{
    [RequireComponent(typeof(LineRenderer))]
    public class DefaultActionView : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        [SerializeField] private BoatAction _boatAction;

        private IEnumerator _actionEffectRoutine;

        void Start ()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _boatAction.NotifyBoatAction += OnNotifyAction;
        }

        void OnDestroy()
        {
            _boatAction.NotifyBoatAction -= OnNotifyAction;
        }

        private void OnNotifyAction(object sender, BoatActionArg actionArg)
        {
            if (_actionEffectRoutine != null)
                StopCoroutine(_actionEffectRoutine);
            _actionEffectRoutine = ActionEffect(actionArg);
            StartCoroutine(_actionEffectRoutine);
        }

        private IEnumerator ActionEffect(BoatActionArg actionArg)
        {
            _lineRenderer.enabled = true;
            for  (float progress = 0; progress <= 1; progress = (Time.timeSinceLevelLoad - actionArg.Date) / actionArg.Frequency)
            {
                var distance = Vector3.Distance(transform.position, actionArg.Position);
                _lineRenderer.SetPositions(new []{transform.position, actionArg.Position });
                _lineRenderer.material.mainTextureOffset = new Vector2(progress, 0);
                _lineRenderer.material.mainTextureScale = new Vector2(distance / 2, 1);

                var color = _lineRenderer.material.GetColor("_TintColor");
                color.a = 1 - progress;
                _lineRenderer.material.SetColor("_TintColor", color);

                yield return null;
            }
            _lineRenderer.enabled = false;
            _actionEffectRoutine = null;
        }
    }
}
