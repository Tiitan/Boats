using UnityEngine;

namespace Controllers
{
    public class ControllerManager : MonoBehaviour
    {
        private IController _currentController;
        private IController _defaultController;

        void Start()
        {
            _defaultController = LevelManager.Instance.PlayerControl;
        }

        void Update()
        {
            if (_currentController != null)
                _currentController.ControllerUpdate();
            else
                _defaultController.ControllerUpdate();
        }

        /// <summary>
        /// instantaneously switch the control to a new controller
        /// </summary>
        /// <param name="controller">new controlling controller</param>
        public void ClaimControl(IController controller)
        {
            if (_currentController == controller) return;
            _currentController?.LostFocus();
            _currentController = controller;
            controller.GotFocus();
        }

        public void UnRegister(IController controller)
        {
            if (_currentController != controller) return;
            _currentController?.LostFocus();
            _currentController = null;
        }
    }
}
