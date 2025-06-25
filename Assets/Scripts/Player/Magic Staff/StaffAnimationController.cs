using UnityEngine;
using UnityEngine.InputSystem;

public class StaffAnimationController : MonoBehaviour
{
    [SerializeField] private InputReader _reader;

    private Animator _animator;

    private void OnEnable()
    {
        _reader.AttackEvent += SetShootBool;
    }

    private void OnDisable()
    {
        _reader.AttackEvent -= SetShootBool;
    }

    private void Start()
    {
        TryGetComponent<Animator>(out _animator);
    }

    private void SetShootBool(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

            _animator.SetTrigger("shoot");
        }
        //if (context.phase == InputActionPhase.Canceled)
        //{
        //    _animator.ResetTrigger("shoot");
        //    _animator.SetBool("stopShooting", true);
        //}
    }


}
