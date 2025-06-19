using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public string AttackName;

    [SerializeField] private GameObject _attackPrefab;
    private GameObject _attackInstance;

    [Header("Values for initializing attack parameters")]
    [SerializeField] private float _healthDamage;
    [SerializeField] private bool _instantDamage;
    [SerializeField] private float _healthDamageDuration;

    [SerializeField] private bool _doesSpeedDamage;
    [SerializeField] private float _speedDamage;
    [SerializeField] private float _speedDamageDuration;

    [SerializeField] private float _launchSpeed;
    [SerializeField] private bool _useGravity;

    public AttackSO()
    {
        AttackName = "";
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
