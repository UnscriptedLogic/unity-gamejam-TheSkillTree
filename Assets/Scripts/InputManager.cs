using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    [SerializeField] private DefaultControls defaultControls;

    public Vector2 MousePosition { get; private set; }
    public Action OnClick;
    public Action OnCancel;

    public static InputManager instance; 

    private void Awake()
    {
        instance = this;
        defaultControls = new DefaultControls();
    }

    private void OnEnable()
    {
        defaultControls.Enable();
        defaultControls.DefaultMap.Enable();

        defaultControls.DefaultMap.OnClick.performed += OnClick_performed;
        defaultControls.DefaultMap.MousePosition.performed += MousePosition_performed;
        defaultControls.DefaultMap.CancelOperation.performed += CancelOperation_performed;
    }

    private void CancelOperation_performed(InputAction.CallbackContext obj) => OnCancel?.Invoke();

    private void MousePosition_performed(InputAction.CallbackContext obj) => MousePosition = obj.ReadValue<Vector2>();

    public bool IsMouseOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = MousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnClick_performed(InputAction.CallbackContext obj) => OnClick?.Invoke();

    private void OnDisable()
    {
        defaultControls.Disable();
        defaultControls.DefaultMap.Disable();
    }
}
