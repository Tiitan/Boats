#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using System.Collections;
using UnityEngine;

namespace Effect
{
    [RequireComponent(typeof(LineRenderer))]
    public class HarvestView : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        [SerializeField] private Harvester _harvester;

        private IEnumerator _harvestEffectRoutine;

        void Start ()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _harvester.NotifyHarvest += OnNotifyHarvest;
        }

        void OnDestroy()
        {
            _harvester.NotifyHarvest -= OnNotifyHarvest;
        }

        private void OnNotifyHarvest(object sender, HarvestArg harvestArg)
        {
            if (_harvestEffectRoutine != null)
                StopCoroutine(_harvestEffectRoutine);
            _harvestEffectRoutine = HarvestEffect(harvestArg);
            StartCoroutine(_harvestEffectRoutine);
        }

        private IEnumerator HarvestEffect(HarvestArg harvestArg)
        {
            _lineRenderer.enabled = true;
            for  (float progress = 0; progress <= 1; progress = (Time.timeSinceLevelLoad - harvestArg.Date) / harvestArg.Frequency)
            {
                var distance = Vector3.Distance(transform.position, harvestArg.Position);
                _lineRenderer.SetPositions(new []{transform.position, harvestArg.Position });
                _lineRenderer.material.mainTextureOffset = new Vector2(progress, 0);
                _lineRenderer.material.mainTextureScale = new Vector2(distance / 2, 1);

                var color = _lineRenderer.material.GetColor("_TintColor");
                color.a = 1 - progress;
                _lineRenderer.material.SetColor("_TintColor", color);

                yield return null;
            }
            _lineRenderer.enabled = false;
            _harvestEffectRoutine = null;
        }
    }
}
