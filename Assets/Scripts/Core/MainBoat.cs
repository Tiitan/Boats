#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using Core.BoatActions;
using Core.Controllers;
using Core.EventArgs;
using Enums;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Harvester), typeof(Builder))]
    [RequireComponent(typeof(Inventory))]
    public class MainBoat : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private PlayerControl _controller;
        private Harvester _harvester;
        private Builder _builder;
        private Inventory _inventory;

        private Selectable _target;

        void Start ()
        {
            _controller = LevelManager.Instance.PlayerControl;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _harvester = GetComponent<Harvester>();
            _builder = GetComponent<Builder>();
            _inventory = GetComponent<Inventory>();

            _controller.TargetLocationChanged += OnTargetLocationChanged;
            _controller.TargetCommandChanged += OnTargetCommandChanged;

            CoreContainerViewModel.Instance.MainInventory = _inventory;
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
                    if (_harvester.TryExecute(_target.GetComponent<Resource>(), _inventory))
                        _target = null;
                    break;
                case EntityType.Structure:
                    if (_builder.TryExecute(_target.GetComponent<Blueprint>(), _inventory))
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
            // Debug.Log($"New target location: {e.Location}");
            _navMeshAgent.SetDestination(e.Location);
        }
    }
}
