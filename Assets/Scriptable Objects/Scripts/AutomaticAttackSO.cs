using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "AutomaticAttackSO", menuName = "Scriptable Objects/AutomaticAttackSO")]
public class AutomaticAttackSO : AbilityBaseSO
{

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

    public AutomaticAttackSO()
    {
        AbilityName = "";
    }
    public override void TriggerAbility(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
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
    public void StartInstant(Transform instPoint, Vector3 shootDirection)
    {
        InstantAttack attackComponent = InitializeAttack<InstantAttack>(instPoint);
        attackComponent.LaunchAttack(shootDirection);
    }

    private IEnumerator FireAutomatic(Transform instPoint)
    {
        while (_automaticActive)
        {
            StartInstant(instPoint, _playerWorldInfo.PlayerCamOrientation);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
