namespace EventArgs
{
    /// <summary>
    /// Used in event that notify a build cursor to update its renderer  
    /// </summary>
    public class CanBuildAtLocationChangedArg : System.EventArgs
    {
        public bool NewState { get; }

        public CanBuildAtLocationChangedArg(bool newState)
        {
            NewState = newState;
        }
    }
}