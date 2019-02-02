using UnityEngine;

namespace Core.EventArgs
{
    /// <summary>
    /// Message between PlayerControl and Mainboat when a new target is selected
    /// </summary>
    public class TargetChangedArg : System.EventArgs
    {
        public Vector3 Location { get; }
        public Selectable Target { get; }

        public TargetChangedArg(Vector3 location, Selectable target)
        {
            Location = location;
            Target = target;
        }
    }
}