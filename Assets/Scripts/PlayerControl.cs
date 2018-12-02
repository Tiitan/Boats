using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetChangedArg : EventArgs
{
    public Vector3 Location { get; }
    public Targetable Target { get; }

    public TargetChangedArg(Vector3 location, Targetable target)
    {
        Location = location;
        Target = target;
    }
}

public class PlayerControl : MonoBehaviour
{

    private Vector3 _targetLocation;
    private const double LocationChangedMinDistance = 0.5f; 

    public event EventHandler<TargetChangedArg> TargetLocationChanged;
    public event EventHandler<TargetChangedArg> TargetCommandChanged;
	
	void Update ()
    {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject()  &&
            Physics.Raycast(ray, out var hitInfo))
        {
            var target = hitInfo.transform.GetComponent<Targetable>();
            if (Input.GetButton("Mouse1") && Vector3.Distance(_targetLocation, hitInfo.point) > LocationChangedMinDistance)
            {
                TargetLocationChanged?.Invoke(this, new TargetChangedArg(hitInfo.point, target));
                _targetLocation = hitInfo.point;
            }
            if (Input.GetButton("Mouse2") && target != null)
            {
                TargetCommandChanged?.Invoke(this, new TargetChangedArg(hitInfo.point, target));
            }
        }
    }
}

