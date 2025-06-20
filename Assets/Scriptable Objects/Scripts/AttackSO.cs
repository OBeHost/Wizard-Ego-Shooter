using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public string AttackName;

    [SerializeField] private GameObject _attackPrefab;

    private BaseAttack _attackComponent;
    private GameObject _attackInstance;

    [Header("Values for initializing attack parameters")]
    [SerializeField] public AttackType AttackType;
    [SerializeField] private float _healthDamage;
    [SerializeField] private bool _instantDamage;
    [SerializeField] private float _healthDamageDuration;

    [SerializeField] private bool _doesSpeedDamage;
    [SerializeField] private float _speedDamage;
    [SerializeField] private float _speedDamageDuration;

    [SerializeField] private float _launchSpeed;
    [SerializeField] private bool _useGravity;

    private bool _automaticActive = false;

    public AttackSO()
    {
        AttackName = "";
    }

    public void StartInstant(Transform instPoint, Vector3 shootDirection)
    {
        _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);

        _attackComponent = _attackInstance.AddComponent<InstantAttack>();

        _attackComponent.Init(
                AttackType,
                _healthDamage,
                _instantDamage,
                _healthDamageDuration,
                _doesSpeedDamage,
                _speedDamage,
                _speedDamageDuration,
                _launchSpeed,
                _useGravity);

        _attackComponent.LaunchAttack(shootDirection);
    }

    public void StartChargeable(Transform instPoint, Vector3 shootDirection)
    {
        if (_attackInstance == null)
        {
            _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
            _attackComponent = _attackInstance.AddComponent<ChargeableAttack>();
            _attackComponent.Init(
                    AttackType,
                    _healthDamage,
                    _instantDamage,
                    _healthDamageDuration,
                    _doesSpeedDamage,
                    _speedDamage,
                    _speedDamageDuration,
                    _launchSpeed,
                    _useGravity);
        }


        _attackComponent.ChargeAttack(instPoint);
    }

    public void ReleaseChargeable(Vector3 shootDirection)
    {
        if (_attackComponent == null) return;

        _attackComponent.LaunchAttack(shootDirection);
    }

    public void StartStream(Transform instPoint)
    {
        if (_attackInstance == null)
        {
            _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
            _attackComponent = _attackInstance.AddComponent<StreamAttack>();
            _attackComponent.Init(
                    AttackType,
                    _healthDamage,
                    _instantDamage,
                    _healthDamageDuration,
                    _doesSpeedDamage,
                    _speedDamage,
                    _speedDamageDuration,
                    _launchSpeed,
                    _useGravity);
        }

        _attackComponent.LaunchStream(instPoint);
    }

    public void CancleStream()
    {
        if (_attackComponent == null) return;

        _attackComponent.CancleAttack();
    }

    public void StartAutomatic(Transform instPoint, Vector3 shootDirection)
    {

    }


    public void CancleAutomatic()
    {
        _automaticActive = false;
    }

    public void PrepareAttack(Transform instPoint)
    {
        _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        BaseAttack attack = _attackInstance.GetComponent<BaseAttack>();
        attack.SetParent(instPoint);
    }

    public void TriggerAttack(Transform instPoint, Vector3 shootDirection)
    {
        //_attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        BaseAttack attack = _attackInstance.GetComponent<BaseAttack>();

        attack.Init(
                AttackType,
                _healthDamage,
                _instantDamage,
                _healthDamageDuration,
                _doesSpeedDamage,
                _speedDamage,
                _speedDamageDuration,
                _launchSpeed,
                _useGravity);


        attack.LaunchAttack(shootDirection);
    }
}

public enum AttackType
{
    Instant,
    Chargeable,
    Stream, 
    Automatic
}
