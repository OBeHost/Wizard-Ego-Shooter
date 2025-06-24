using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAttack : MonoBehaviour
{
    protected Rigidbody _rb;

    protected AttackType _attackType;
    protected float _healthDamage;
    protected bool _instantDamage;
    protected float _healthDamageDuration;
    protected float _damageRadius;

    protected bool _doesSpeedDamage;
    protected float _speedDamage;
    protected float _speedDamageDuration;

    protected float _launchSpeed;
    protected float _flyTime;
    protected bool _useGravity;

    protected bool _hasLaunched;

    public void Init(
                    AttackType attackType,
                    float healthDamage,
                    bool instantDamage,
                    float healthDamageDuration,
                    float damageRadius,
                    bool doesSpeedDamage,
                    float speedDamage,
                    float speedDamageDuration,
                    float launchSpeed,
                    float flyTime,
                    bool useGravity)
    {
        this._attackType = attackType;
        this._healthDamage = healthDamage;
        this._instantDamage = instantDamage;
        this._healthDamageDuration = healthDamageDuration;
        this._damageRadius = damageRadius;

        this._doesSpeedDamage = doesSpeedDamage;
        this._speedDamage = speedDamage;
        this._speedDamageDuration = speedDamageDuration;

        this._launchSpeed = launchSpeed;
        this._flyTime = flyTime;
        this._useGravity = useGravity;


    }

    protected void OnTriggerEnter(Collider other)
    {
        Explode(transform.position, _damageRadius);
    }

    protected void Explode(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<IDamageable>() == null) continue;
            collider.GetComponent<IDamageable>().InflictDamage(_healthDamage);
        }
        Destroy(gameObject);
    }

    public void InitializeRB()
    {
        _rb = this.gameObject.GetOrAdd<Rigidbody>();
        _rb.useGravity = _useGravity;
    }

    public void SetParent(Transform parent)
    {
        this.transform.parent = parent;
    }

    protected void CalculateFlyTime()
    {
        if (_hasLaunched)
        {
            _flyTime -= Time.deltaTime;
            if (_flyTime <= 0) Destroy(gameObject);
        }
    }

    public abstract void OnStarted();
    public abstract void OnPerformed();
    public abstract void OnCanceled();

}
