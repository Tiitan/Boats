using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    public PlayerControl Control { get; private set; }

    void Awake ()
	{
	    Instance = this;
	    Control = gameObject.GetComponentInChildren<PlayerControl>();
	}
}
