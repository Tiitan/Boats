using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(SphereCollider))]
    public class Selector : MonoBehaviour
    {
        private SphereCollider _collider;
        private readonly List<Selectable> _selectedTargets = new List<Selectable>();

        public IEnumerable<Selectable> SelectedTargets => _selectedTargets;

        public float SelectionRadius
        {
            get => _collider.radius;
            set => _collider.radius = value;
        }

        private void Start()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.gameObject.GetComponent<Selectable>() ?? other.gameObject.GetComponentInParent<Selectable>();
            if (target != null)
            {
                _selectedTargets.Add(target);
                target.Select(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var target = other.gameObject.GetComponent<Selectable>() ?? other.gameObject.GetComponentInParent<Selectable>();
            if (target != null)
            {
                _selectedTargets.Remove(target);
                target.Select(false);
            }
        }
    }
}
