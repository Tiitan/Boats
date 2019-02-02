using Core.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public class LevelManager : MonoBehaviour
    {

        public static LevelManager Instance { get; private set; }

        public PlayerControl PlayerControl { get; private set; }
        public ControllerManager ControllerManager { get; private set; }
        public NavMeshSurface NavMeshSurface { get; private set; }
        public StationsManager StationsManager { get; private set; }

        void Awake ()
        {
            Instance = this;
            PlayerControl = gameObject.GetComponentInChildren<PlayerControl>();
            NavMeshSurface = gameObject.GetComponentInChildren<NavMeshSurface>();
            StationsManager = gameObject.GetComponentInChildren<StationsManager>();
            ControllerManager = gameObject.GetComponentInChildren<ControllerManager>();
        }
    }
}
