using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : AbilityBaseSO
{
    //public string AttackName;

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
        AbilityName = "";
    }

    public void StartInstant(Transform instPoint, Vector3 shootDirection)
    {
        InitializeAttack<InstantAttack>(instPoint);

        _attackComponent.LaunchAttack(shootDirection);
    }

    public void StartChargeable(Transform instPoint, Vector3 shootDirection)
    {
        if (_attackInstance == null)
        {
            InitializeAttack<ChargeableAttack>(instPoint);
        }


        _attackComponent.ChargeAttack(instPoint);
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
            InitializeAttack<StreamAttack>(instPoint);
        }

        _attackComponent.LaunchStream(instPoint);
    }

    public void CancleStream()
    {
        if (_attackComponent == null) return;

        _attackComponent.CancleAttack();
    }

    private void InitializeAttack<T>(Transform instPoint) where T : BaseAttack
    {
        _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        _attackComponent = _attackInstance.AddComponent<T>();

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

}

public enum AttackType
{
    Instant,
    Chargeable,
    Stream, 
    Automatic
}
