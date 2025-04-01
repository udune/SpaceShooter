using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private InputActionProperty rotateAction;

    private void OnEnable()
    {
        inputActions.FindAction("Player/Move").Enable();
        inputActions.FindAction("Fire").Enable();
        rotateAction.action.Enable();
    }

    private void OnDisable()
    {
        inputActions.FindAction("Player/Move").Disable();
        inputActions.FindAction("Fire").Disable();
        rotateAction.action.Disable();
    }

    public event Action<Vector2> OnMove;
    public event Action<bool> OnFire;
    public event Action<float> OnRotate;

    private void Start()
    {
        inputActions.FindAction("Move").performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        inputActions.FindAction("Move").canceled += ctx => OnMove?.Invoke(Vector2.zero);
        
        inputActions.FindAction("Fire").performed += ctx => OnFire?.Invoke(true);
        inputActions.FindAction("Fire").canceled += ctx => OnFire?.Invoke(false);
        
        rotateAction.action.performed += ctx => OnRotate?.Invoke(ctx.ReadValue<float>());
        rotateAction.action.canceled += ctx => OnRotate?.Invoke(0.0f);
    }
}
