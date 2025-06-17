using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject
{
    [SerializeField] private InputActionAsset _asset;

    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction<Vector2> LookEvent;
    public event UnityAction<InputAction.CallbackContext> JumpEvent;
    public event UnityAction<InputAction.CallbackContext> SprintEvent;
    public event UnityAction InteractEvent;
    public event UnityAction AttackEvent;
    public event UnityAction EscapeEvent;
    public event UnityAction<Vector2> ScrollEvent;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _jumpAction;
    private InputAction _interactionAction;
    private InputAction _attackAction;
    private InputAction _escapeAction;
    private InputAction _scrollAction;

    public bool IsAttacking = false;
    public bool IsSprinting = false;

    private void OnEnable()
    {
        _moveAction = _asset.FindAction("Move");
        _interactionAction = _asset.FindAction("Interact");
        _attackAction = _asset.FindAction("Attack");
        _lookAction = _asset.FindAction("Look");
        _sprintAction = _asset.FindAction("Sprint");
        _jumpAction = _asset.FindAction("Jump");
        _scrollAction = _asset.FindAction("ScrollWheel");

        _moveAction.started += OnMove;
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;

        _jumpAction.started += OnJump;
        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;

        _lookAction.started += OnLook;
        _lookAction.performed += OnLook;

        _sprintAction.started += OnSprint;
        _sprintAction.performed += OnSprint;
        _sprintAction.canceled += OnSprint;

        _interactionAction.started += OnInteract;

        _attackAction.started += OnAttack;
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;

        _scrollAction.performed += OnScroll;


        _attackAction.Enable();
        _interactionAction.Enable();
        _moveAction.Enable();
        _lookAction.Enable();
        _sprintAction.Enable();
        _jumpAction.Enable();
        _scrollAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.started -= OnMove;
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;

        _jumpAction.started -= OnJump;
        _jumpAction.performed -= OnJump;
        _jumpAction.canceled -= OnJump;

        _lookAction.started -= OnLook;
        _lookAction.performed -= OnLook;

        _sprintAction.started -= OnSprint;
        _sprintAction.performed -= OnSprint;
        _sprintAction.canceled -= OnSprint;

        _interactionAction.started -= OnInteract;

        _attackAction.started -= OnAttack;
        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;

        _scrollAction.performed -= OnScroll;


        _attackAction.Disable();
        _interactionAction.Disable();
        _moveAction.Disable();
        _lookAction.Disable();
        _sprintAction.Disable();
        _jumpAction.Disable();
        _scrollAction.Disable();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            IsAttacking = true;
        } else { IsAttacking = false; }
        AttackEvent?.Invoke();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnEscape(InputAction.CallbackContext context)
    {
        EscapeEvent?.Invoke();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        SprintEvent?.Invoke(context);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke(context);
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        Vector2 scrollDir = context.ReadValue<Vector2>();
        if (scrollDir == Vector2.zero) return;
        ScrollEvent?.Invoke(scrollDir);        
    }
}
