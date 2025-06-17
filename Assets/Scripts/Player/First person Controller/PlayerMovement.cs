using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _reader;

    [Header("Movement")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _sprintSpeed;
    private float _moveSpeed;

    [SerializeField] private float _groundDrag;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airMultiplier;

    private bool _readyToJump = true;
    private bool _jumpIsPressed = false;
    private bool _isSprinting = false;


    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayerMask;
    private float _playerHeight;
    private bool _grounded;

    //Stores flat direction in which to walk
    [SerializeField] private Transform _orientation;

    private float _xInput;
    private float _yInput;

    private Vector3 _moveDir;

    private Rigidbody _rb;

    private void OnEnable()
    {
        _reader.MoveEvent += SetMoveDirection;
        _reader.JumpEvent += SetJump;
        _reader.SprintEvent += SetSprint;
    }

    private void OnDisable()
    {
        _reader.MoveEvent -= SetMoveDirection;
        _reader.JumpEvent -= SetJump;
        _reader.SprintEvent -= SetSprint;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _playerHeight = this.transform.localScale.y;
    }

    private void FixedUpdate()
    {
        if (_jumpIsPressed)
        {
            Jump();
        }
        MovePlayer();
    }

    private void Update()
    {
        _grounded = Physics.Raycast(this.transform.position, Vector3.down, _playerHeight * 0.5f + 1f, _groundLayerMask);

        if (_grounded)
        {
            _rb.linearDamping = _groundDrag;
        } else
        {
            _rb.linearDamping = 0;
        }
        SpeedControl();
    }

    private void SetMoveDirection(Vector2 inputDir)
    {
        _xInput = inputDir.x;
        _yInput = inputDir.y;
    }

    private void MovePlayer()
    {
        _moveDir = _orientation.forward * _yInput + _orientation.right * _xInput;

        _moveSpeed = _isSprinting ? _sprintSpeed : _normalSpeed;

        switch (_grounded)
        {
            case true:
                _rb.AddForce(_moveDir.normalized * _moveSpeed * 10f, ForceMode.Force);
                break;
            case false:
                _rb.AddForce(_moveDir.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
                break;
        }

        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }
    }

    #region Event Subscriptions
    private void SetSprint(InputAction.CallbackContext context)
    {
        _isSprinting = context.action.IsPressed();
    }
    private void SetJump(InputAction.CallbackContext context)
    {
        _jumpIsPressed = context.action.IsPressed();
    }
    #endregion

    private void Jump()
    {
        if (!_readyToJump || !_grounded) return;

        _readyToJump = false;

        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

        _readyToJump = true;
    }
}
