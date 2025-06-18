using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    [SerializeField] private BaseAttack _attack;
    private GameObject _attackInstance;

    [SerializeField] private float _healthDamage;
    [SerializeField] private bool _instantDamage;
    [SerializeField] private float _healthDamageDuration;

    [SerializeField] private bool _doesSpeedDamage;
    [SerializeField] private float _speedDamage;
    [SerializeField] private float _speedDamageDuration;

    [SerializeField] private bool _useGravity;

    public string AttackName;
    public AttackSO()
    {
        AttackName = "";
    }

    public void TriggerAttack(Transform instPoint)
    {
        _attackInstance = Instantiate(_attack.gameObject, instPoint.position, instPoint.rotation);
        _attackInstance.GetComponent<BaseAttack>().InitializeAttack();
    }
}
