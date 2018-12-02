#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField

using UI;
using UI.ViewModel;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private CommandViewModel _buildCommand;

    private GameObject _cursor;

    [SerializeField] private GameObject _cursorPrefab;
    [SerializeField] private GameObject _blueprintPrefab;


    private void ToogleBuildManager(bool e)
    {
        LevelManager.Instance.Control.enabled = !e;
        enabled = e;
        if (e)
            _cursor = Instantiate(_cursorPrefab);
        else if (_cursor)
            Destroy(_cursor);
    }

    private void Start()
    {
        _buildCommand = new CommandViewModel("Build platform", OnBuildCommand);
        UiManager.Instance.CommandUiView.Register(_buildCommand);
        enabled = false;
    }

    private void OnDestroy()
    {
        if (UiManager.Instance && UiManager.Instance.CommandUiView)
            UiManager.Instance.CommandUiView.UnRegister(_buildCommand);
    }

    private void OnBuildCommand()
    {
        ToogleBuildManager(true);        
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            _cursor.transform.position = hitInfo.point;
            if (Input.GetButton("Mouse1"))
            {
                Instantiate(_blueprintPrefab, _cursor.transform.position, _cursor.transform.rotation);
                ToogleBuildManager(false);
            }
        }
        if (Input.GetButton("Mouse2"))
        {
            ToogleBuildManager(false);
        }
    }
}
