using System;
using Core.EventArgs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Controllers
{
    public class PlayerControl : MonoBehaviour, IController
    {

        private Vector3 _targetLocation;
        private const double LocationChangedMinDistance = 0.5f; 

        public event EventHandler<TargetChangedArg> TargetLocationChanged;
        public event EventHandler<TargetChangedArg> TargetCommandChanged;

        public void ControllerUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject()  &&
                Physics.Raycast(ray, out var hitInfo))
            {
                var target = hitInfo.transform.GetComponent<Selectable>();
                if (Input.GetButton("BoatMove") && Vector3.Distance(_targetLocation, hitInfo.point) > LocationChangedMinDistance)
                {
                    TargetLocationChanged?.Invoke(this, new TargetChangedArg(hitInfo.point, target));
                    _targetLocation = hitInfo.point;
                }
                if (Input.GetButton("BoatAction") && target != null)
                {
                    TargetCommandChanged?.Invoke(this, new TargetChangedArg(hitInfo.point, target));
                }
            }
        }

        public void GotFocus()
        {

        }

        public void LostFocus()
        {
            
        }
    }
}

