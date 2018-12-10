namespace EventArgs
{
    /// <summary>
    /// Used in event that notify a build cursor to update its renderer  
    /// </summary>
    public class CanBuildAtLocationChangedArg : System.EventArgs
    {
        public bool IsObstructed { get; }
        public bool IsTooClose { get; }

        public CanBuildAtLocationChangedArg(bool isObstructed, bool isTooClose)
        {
            IsObstructed = isObstructed;
            IsTooClose = isTooClose;
        }
    }
}