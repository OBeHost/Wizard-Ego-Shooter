using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _reader;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundDrag;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCoolDown;
    [SerializeField] private float _airMultiplier;
    private bool _readyToJump = true;
    private bool _isJumping = false;


    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayerMask;

    [SerializeField] private Transform _orientation;

    private float _xInput;
    private float _yInput;

    private Vector3 _moveDir;

    private Rigidbody _rb;

    private bool _grounded;
    private void OnEnable()
    {
        _reader.MoveEvent += SetMoveDirection;
        _reader.JumpEvent += SetJump;
    }

    private void OnDisable()
    {
        _reader.MoveEvent -= SetMoveDirection;
        _reader.JumpEvent -= SetJump;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            Jump();
        }
        MovePlayer();
    }

    private void Update()
    {
        _grounded = Physics.Raycast(this.transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayerMask);

        if (_grounded)
        {
            _rb.linearDamping = _groundDrag;
        } else
        {
            _rb.linearDamping = 0;
        }

    }

    private void SetMoveDirection(Vector2 inputDir)
    {
        _xInput = inputDir.x;
        _yInput = inputDir.y;
    }

    private void MovePlayer()
    {
        _moveDir = _orientation.forward * _yInput + _orientation.right * _xInput;

        switch (_grounded)
        {
            case true:
                _rb.AddForce(_moveDir.normalized * _moveSpeed * 10f, ForceMode.Force);
                break;
            case false:
                _rb.AddForce(_moveDir.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
                break;
        }

        SpeedControl();
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

    private void SetJump(InputAction.CallbackContext context)
    {
        _isJumping = context.action.IsPressed();
    }

    private void Jump()
    {
        if (!_readyToJump || !_grounded) return;

        _readyToJump = false;

        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), _jumpCoolDown);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }
}
