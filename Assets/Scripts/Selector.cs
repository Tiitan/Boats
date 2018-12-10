using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Selector : MonoBehaviour
{
    private SphereCollider _collider;

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
            target.Select(true);
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.gameObject.GetComponent<Selectable>() ?? other.gameObject.GetComponentInParent<Selectable>();
        if (target != null)
            target.Select(false);
    }
}
