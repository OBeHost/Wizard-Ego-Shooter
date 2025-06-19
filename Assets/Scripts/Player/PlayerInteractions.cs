using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private InputReader _reader;
    [SerializeField] private Transform _cameraOrientation;

    [Header("Attacking")]
    [SerializeField] private CurrentAttackHolderSO _attackHolder;
    [SerializeField] private Transform _projectileInstantiationPoint;

    private bool _holdingAttack = false;

    private void OnEnable()
    {
        _reader.AttackEvent += Attack;
    }

    private void OnDisable()
    {
        _reader.AttackEvent -= Attack;
    }

    private void Update()
    {
        if (_reader.HoldingAttack)
        {
            
        }
    }




    private void Attack(InputAction.CallbackContext context)
    {
        AttackSO attack = _attackHolder.CurrentAttack;
        if (context.phase == InputActionPhase.Started)
        {
            attack.PrepareAttack(_projectileInstantiationPoint);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            print("Attack released");
            Vector3 lookDirection = _cameraOrientation.forward;
            attack.TriggerAttack(_projectileInstantiationPoint, lookDirection);
        }
    }
}
