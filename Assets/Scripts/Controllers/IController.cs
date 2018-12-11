namespace Controllers
{
    public interface IController
    {
        /// <summary>
        /// ControllerManager state controlled Unity Update
        /// </summary>
        void ControllerUpdate();

        /// <summary>
        /// The control was takken over by another controller or the current controller self unregistered
        /// </summary>
        void LostFocus();

        /// <summary>
        /// This controller is now active
        /// </summary>
        void GotFocus();
    }
}
