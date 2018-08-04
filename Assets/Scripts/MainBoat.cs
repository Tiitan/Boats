#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using UI;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Harvester))]
public class MainBoat : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private PlayerControl _controller;
    private Harvester _harvester;

    [SerializeField] private Inventory _inventory;

    private Targetable _target;

	void Start ()
	{
	    _controller = LevelManager.Instance.Control;
	    _navMeshAgent = GetComponent<NavMeshAgent>();
	    _harvester = GetComponent<Harvester>();

        _controller.TargetLocationChanged += OnTargetLocationChanged;
	    _controller.TargetCommandChanged += OnTargetCommandChanged;

	    UiManager.Instance.Inventory.Initialize(_inventory.Items);
    }

    void OnDestroy()
    {
        _controller.TargetLocationChanged -= OnTargetLocationChanged;
        _controller.TargetCommandChanged -= OnTargetCommandChanged;
    }

    void Update()
    {
        if (_target == null)
            return;

        switch (_target.Type)
        {
            case EntityType.Resource:
                if (_harvester.Harvest(_target.GetComponent<Resource>(), _inventory))
                    _target = null;
                break;
        }
    }

    private void OnTargetCommandChanged(object sender, TargetChangedArg e)
    {
        _target = e.Target;
    }

    private void OnTargetLocationChanged(object sender, TargetChangedArg e)
    {
        Debug.Log($"New target location: {e.Location}");
        _navMeshAgent.SetDestination(e.Location);
    }
}
