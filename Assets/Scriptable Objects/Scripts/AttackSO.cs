using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : AbilityBaseSO
{
    //public string AttackName;

    [SerializeField] private GameObject _attackPrefab;


    [SerializeField] private PlayerWorldInfo _playerWorldInfo;

    private BaseAttack _attackComponent;
    private GameObject _attackInstance;

    [Header("Values for initializing attack parameters")]
    [SerializeField] public AttackType AttackType;
    [SerializeField] private float _healthDamage;
    [SerializeField] private bool _instantDamage;
    [SerializeField] private float _healthDamageDuration;
    [SerializeField] private float _damageRadius = 1f;

    [SerializeField] private bool _doesSpeedDamage;
    [SerializeField] private float _speedDamage;
    [SerializeField] private float _speedDamageDuration;

    [SerializeField] private float _launchSpeed;
    [SerializeField] private float _flyTime;
    [SerializeField] private bool _useGravity;

    private bool _automaticActive = false;
    private float _timer = 0.1f;

    public AttackSO()
    {
        AbilityName = "";
    }

    public void StartInstant(Transform instPoint, Vector3 shootDirection)
    {
        InstantAttack attackComponent = InitializeAttack<InstantAttack>(instPoint);
        attackComponent.LaunchAttack(shootDirection);
    }

    public void StartChargeable(Transform instPoint, Vector3 shootDirection)
    {
        if (_attackInstance != null) return;

        ChargeableAttack attackComponent = InitializeAttack<ChargeableAttack>(instPoint);
        _attackComponent = attackComponent;


        attackComponent.ChargeAttack(instPoint);
    }

    public void ReleaseChargeable(Vector3 shootDirection)
    {
        if (_attackComponent == null) return;

        _attackComponent.LaunchAttack(shootDirection);
        _attackInstance = null;
        _attackComponent = null;
    }

    public void StartStream(Transform instPoint)
    {
        if (_attackInstance == null)
        {
            StreamAttack attackComponent = InitializeAttack<StreamAttack>(instPoint);
            _attackComponent = attackComponent;
        }
        _attackComponent.LaunchAttack(Vector3.zero, instPoint);
    }

    public void CancleStream()
    {
        if (_attackComponent == null) return;

        StreamAttack attackComponent = _attackInstance.GetComponent<StreamAttack>();
        attackComponent.CancleAttack();
    }



    private T InitializeAttack<T>(Transform instPoint) where T : BaseAttack
    {
        _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        T attackComponent = _attackInstance.AddComponent<T>();

        attackComponent.Init(
                AttackType,
                _healthDamage,
                _instantDamage,
                _healthDamageDuration,
                _damageRadius,
                _doesSpeedDamage,
                _speedDamage,
                _speedDamageDuration,
                _launchSpeed,
                _flyTime,
                _useGravity);

        return attackComponent as T;
        
    }

    private IEnumerator FireAutomatic(Transform instPoint)
    {
        while (_automaticActive)
        {
            StartInstant(instPoint, _playerWorldInfo.PlayerCamOrientation);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void TriggerAbility(InputAction.CallbackContext context)
    {
        // TODO: Implement so that the attack SO can handle the logic for spawning and moving projectile on its own

        switch (AttackType)
        {
            case AttackType.Instant:
                if (context.phase == InputActionPhase.Started)
                {
                    StartInstant(_playerWorldInfo.ProjectileInstantiationPoint, _playerWorldInfo.PlayerCamOrientation);
                }
                break;
            case AttackType.Chargeable:
                if (context.phase == InputActionPhase.Started)
                {
                    StartChargeable(_playerWorldInfo.ProjectileInstantiationPoint, _playerWorldInfo.PlayerCamOrientation);
                }
                if (context.phase == InputActionPhase.Canceled)
                {
                    ReleaseChargeable(_playerWorldInfo.PlayerCamOrientation);
                }
                break;
            case AttackType.Stream:
                if (context.phase == InputActionPhase.Started)
                {
                    StartStream(_playerWorldInfo.ProjectileInstantiationPoint);
                }
                if (context.phase == InputActionPhase.Canceled)
                {
                    CancleStream();
                }
                break;
            case AttackType.Automatic:
                if (context.phase == InputActionPhase.Started)
                {
                    _automaticActive = true;
                    CoroutineRunner.Instance.RunCoroutine(FireAutomatic(_playerWorldInfo.ProjectileInstantiationPoint));
                }
                if (context.phase == InputActionPhase.Canceled)
                {
                    _automaticActive = false;
                }
                break;
                default:
                    break;
        }


        //Depending on the phase 
        switch (context.phase)
        {
            case InputActionPhase.Started:
                //attackComponent.OnStarted()
                break;
            case InputActionPhase.Performed:
                //attackComponent.OnPerformed()
                break;
            case InputActionPhase.Canceled:
                //attackComponent.OnCanceled()
                break;
        }
    }
}

public enum AttackType
{
    Instant,
    Chargeable,
    Stream, 
    Automatic
}
