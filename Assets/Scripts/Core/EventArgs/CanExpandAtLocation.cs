namespace Core.EventArgs
{
    /// <summary>
    /// Used in event that notify an expand cursor to update its renderer  
    /// </summary>
    public class CanExpandAtLocationChangedArg : System.EventArgs
    {
        public bool ValidLocation { get; }
        public bool ValidOrientation { get; }

        public CanExpandAtLocationChangedArg(bool validLocation, bool validOrientation)
        {
            ValidLocation = validLocation;
            ValidOrientation = validOrientation;
        }
    }
}