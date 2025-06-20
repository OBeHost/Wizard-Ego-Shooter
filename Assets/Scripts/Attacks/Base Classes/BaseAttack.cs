using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAttack : MonoBehaviour
{
    protected Rigidbody _rb;

    protected AttackType _attackType;
    protected float _healthDamage;
    protected bool _instantDamage;
    protected float _healthDamageDuration;

    protected bool _doesSpeedDamage;
    protected float _speedDamage;
    protected float _speedDamageDuration;

    protected float _launchSpeed;
    protected bool _useGravity;
    public void Init(
                    AttackType attackType,
                    float healthDamage,
                    bool instantDamage,
                    float healthDamageDuration,
                    bool doesSpeedDamage,
                    float speedDamage,
                    float speedDamageDuration,
                    float launchSpeed,
                    bool useGravity)
    {
        this._attackType = attackType;
        this._healthDamage = healthDamage;
        this._instantDamage = instantDamage;
        this._healthDamageDuration = healthDamageDuration;

        this._doesSpeedDamage = doesSpeedDamage;
        this._speedDamage = speedDamage;
        this._speedDamageDuration = speedDamageDuration;

        this._launchSpeed = launchSpeed;
        this._useGravity = useGravity;


    }

    public void InitializeRB()
    {
        _rb = this.gameObject.GetOrAdd<Rigidbody>();
        _rb.useGravity = _useGravity;
    }

    public void SetParent(Transform parent)
    {
        print("ihio");
        this.transform.parent = parent;
    }


    public abstract void LaunchStream(Transform parent);
    public abstract void LaunchAttack(Vector3 direction);
    public abstract void ChargeAttack(Transform parent);
    public abstract void CancleAttack();
    public abstract void LaunchAutomaticAttack();

}
