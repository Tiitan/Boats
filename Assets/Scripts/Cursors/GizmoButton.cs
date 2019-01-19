#pragma warning disable 0649 // Disable "Field is never assigned" for SerializedField
#pragma warning disable IDE0044 // Disable "Add readonly modifier" for SerializedField

using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GizmoButton : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _mouseOverColor;
    [SerializeField] private Color _mouseDownColor;

    private Renderer _renderer;

    private bool _mouseDown;
    private bool _mouseover;

    public event EventHandler<System.EventArgs> GizmoButtonPressed;

    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_mouseDown && Input.GetButtonUp("GizmoSubmit"))
        {
            if (_mouseover)
                GizmoButtonPressed?.Invoke(this, System.EventArgs.Empty);
            _mouseDown = false;
            UpdateMaterial();
        }
        if (_mouseover && Input.GetButtonDown("GizmoSubmit"))
        {
            _mouseDown = true;
            UpdateMaterial();
        }
    }

    /// <summary>
    /// Mouse enter this button
    /// </summary>
    public void OnMouseEnter()
    {
        _mouseover = true;
        UpdateMaterial();
    }

    /// <summary>
    /// Mouse left this button
    /// </summary>
    public void OnMouseExit()
    {
        _mouseover = false;
        UpdateMaterial();
    }



    private void UpdateMaterial()
    {
        _renderer.material.color = _defaultColor;
        if (_mouseover)
        {
            _renderer.material.color = _mouseOverColor;
            if (_mouseDown)
                _renderer.material.color = _mouseDownColor;
        }

    }
}
