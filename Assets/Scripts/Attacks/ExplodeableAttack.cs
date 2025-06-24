using UnityEngine;

public class ExplodeableAttack : MonoBehaviour
{
    private float _damageRadius;
    private float _healthDamage;
    private float _flyTime;

    private bool _hasLaunched = false;


    public void Init(
                     float damageRadius,
                     float healthDamage,
                     float flyTime,
                     bool hasLaunched)
    {
        this._damageRadius = damageRadius;
        this._healthDamage = healthDamage;
        this._flyTime = flyTime;
        this._hasLaunched = hasLaunched;
    }

    private void Update()
    {
        DestroyInFlight();
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode(transform.position, _damageRadius);
    }

    private void Explode(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<IDamageable>() == null) continue;
            collider.GetComponent<IDamageable>().InflictDamage(_healthDamage);
        }
        Destroy(gameObject);
    }


    private void DestroyInFlight()
    {
        if (_hasLaunched)
        {
            _flyTime -= Time.deltaTime;
            if (_flyTime <= 0) Destroy(gameObject);
        }
    }
}
