using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    protected Rigidbody _rb;

    protected float _healthDamage;
    protected bool _instantDamage;
    protected float _healthDamageDuration;

    protected bool _doesSpeedDamage;
    protected float _speedDamage;
    protected float _speedDamageDuration;

    protected float _launchSpeed;
    protected bool _useGravity;
    public void Init(
                    float healthDamage,
                    bool instantDamage,
                    float healthDamageDuration,
                    bool doesSpeedDamage,
                    float speedDamage,
                    float speedDamageDuration,
                    float launchSpeed,
                    bool useGravity)
    {
        this._healthDamage = healthDamage;
        this._instantDamage = instantDamage;
        this._healthDamageDuration = healthDamageDuration;

        this._doesSpeedDamage = doesSpeedDamage;
        this._speedDamage = speedDamage;
        this._speedDamageDuration = speedDamageDuration;

        this._launchSpeed = launchSpeed;
        this._useGravity = useGravity;


        _rb = this.gameObject.GetOrAdd<Rigidbody>();
        _rb.useGravity = useGravity;
        this.transform.parent = null;
    }

    public void SetParent(Transform parent)
    {
        print("ihio");
        this.transform.parent = parent;
    }

   

    public abstract void LaunchAttack(Vector3 direction);



    private void Launch(Vector3 direction)
    {

    }

}
