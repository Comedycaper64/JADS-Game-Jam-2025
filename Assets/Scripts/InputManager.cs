using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    private static bool disableInputs = false;

    public static InputManager Instance { get; private set; }
    private Controls controls;

    public event Action OnAdvanceAction;
    public event Action OnInventoryAction;
    public event Action OnLogAction;
    public event Action OnMenuAction;
    public event Action OnSkipStartAction;
    public event Action OnSkipEndAction;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("There's more than one InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        disableInputs = false;
    }

    private void OnEnable()
    {
        PauseManager.OnPauseGame += ToggleDisableInputs;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseGame -= ToggleDisableInputs;
    }

    public void OnAdvance(InputAction.CallbackContext context)
    {
        // If Pause Active return

        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnAdvanceAction?.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnInventoryAction?.Invoke();
        }
    }

    public void OnLog(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnLogAction?.Invoke();
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMenuAction?.Invoke();
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnSkipStartAction?.Invoke();
        }
        else if (context.canceled)
        {
            OnSkipEndAction?.Invoke();
        }
    }

    private void ToggleDisableInputs(object sender, bool toggle)
    {
        disableInputs = toggle;
    }
}
