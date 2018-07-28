using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectedEffect;

	public void Select (bool isSelected)
    {
        _selectedEffect.SetActive(isSelected);
    }
	
	void Update ()
    {
		
	}
}
