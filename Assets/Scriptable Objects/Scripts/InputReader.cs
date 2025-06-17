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
    public event UnityAction InteractEvent;
    public event UnityAction AttackEvent;
    public event UnityAction EscapeEvent;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _jumpAction;
    private InputAction _interactionAction;
    private InputAction _attackAction;
    private InputAction _escapeAction;

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


        _attackAction.Enable();
        _interactionAction.Enable();
        _moveAction.Enable();
        _lookAction.Enable();
        _sprintAction.Enable();
        _jumpAction.Enable();
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


        _attackAction.Disable();
        _interactionAction.Disable();
        _moveAction.Disable();
        _lookAction.Disable();
        _sprintAction.Disable();
        _jumpAction.Disable();
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
        if (context.action.IsPressed())
        {
            IsSprinting = true;
        } else
        {
            IsSprinting = false;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke(context);
    }
}
