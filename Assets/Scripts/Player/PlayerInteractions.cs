using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private InputReader _reader;
    [SerializeField] private Transform _cameraOrientation;

    [Header("Attacking")]
    [SerializeField] private AbilityHolderSO _abilityHolder;
    [SerializeField] private Transform _projectileInstantiationPoint;

   
    private bool _automaticActive = false;

    private void OnEnable()
    {
        _reader.AttackEvent += Attack;
    }

    private void OnDisable()
    {
        _reader.AttackEvent -= Attack;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        AbilityBaseSO ability = _abilityHolder.CurrentAbility;
        Vector3 lookDirection = _cameraOrientation.forward;

        AttackSO attack = ability as AttackSO;
        if (attack == null) return;

        switch (attack.AttackType)
        {
            case AttackType.Instant:
                if (context.phase == InputActionPhase.Started)
                {
                    attack.StartInstant(_projectileInstantiationPoint, lookDirection);
                }
                break;
            case AttackType.Chargeable:
                if (context.phase == InputActionPhase.Started)
                {
                    attack.StartChargeable(_projectileInstantiationPoint, lookDirection);
                }
                if (context.phase == InputActionPhase.Canceled)
                {
                    attack.ReleaseChargeable(lookDirection);
                }
                break;
            case AttackType.Stream:
                if (context.phase == InputActionPhase.Started)
                {
                    attack.StartStream(_projectileInstantiationPoint);
                }
                if (context.phase == InputActionPhase.Canceled)
                {
                    attack.CancleStream();
                }
                break;
            case AttackType.Automatic:
                if (context.phase == InputActionPhase.Started)
                {
                    _automaticActive = true;
                    StartCoroutine(FireAutomatic(_projectileInstantiationPoint, lookDirection, attack));
                }
                if (context.phase == InputActionPhase.Canceled)
                {
                    _automaticActive = false;
                }
                break;
        }
    }

    private IEnumerator FireAutomatic(Transform instPoint, Vector3 shootDirection, AttackSO attack)
    {
        while (_automaticActive)
        {
            attack.StartInstant(instPoint, shootDirection);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
