using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private Vector3 _targetLocation;
    private const double LocationChangedMinDistance = 0.5f; 

    public event EventHandler<TargetLocationChangedArg> TargetLocationChanged;

    public class TargetLocationChangedArg : EventArgs
    {
        public Vector3 NewTarget { get; }

        public TargetLocationChangedArg(Vector3 newTarget)
        {
            NewTarget = newTarget;
        }
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetButton("Move") && Vector3.Distance(_targetLocation, hitInfo.point) > LocationChangedMinDistance)
            {
                TargetLocationChanged?.Invoke(this, new TargetLocationChangedArg(hitInfo.point));
                _targetLocation = hitInfo.point;
            }
        }
    }
}

