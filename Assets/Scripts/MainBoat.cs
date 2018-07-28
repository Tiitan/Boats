using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MainBoat : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

	void Start ()
    {
        LevelManager.Instance.Control.TargetLocationChanged += TargetLocationChanged;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnDestroy()
    {
        LevelManager.Instance.Control.TargetLocationChanged -= TargetLocationChanged;
    }

    private void TargetLocationChanged(object sender, PlayerControl.TargetLocationChangedArg e)
    {
        Debug.Log($"New target location: {e.NewTarget}");
        _navMeshAgent.SetDestination(e.NewTarget);
    }
}
