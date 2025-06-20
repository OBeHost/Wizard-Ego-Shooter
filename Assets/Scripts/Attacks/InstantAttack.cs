using UnityEngine;
using UnityEngine.InputSystem;

public class InstantAttack : BaseAttack
{

    private bool _hasLaunched = false;
    private float _flyTime = 5f;


    private void Update()
    {
        if (_hasLaunched)
        {
            _flyTime -= Time.deltaTime;
            if (_flyTime <= 0) Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Explode(transform.position, 4f);
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

    public override void LaunchAttack(Vector3 direction)
    {
        Debug.Log("Instantly launched attack");
        InitializeRB();
        _rb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _hasLaunched = true;
    }

    public override void CancleAttack()
    {
        Debug.Log($"Cannot cancle attack of type {_attackType}");
    }

    public override void ChargeAttack(Transform parent)
    {
        Debug.Log($"Attack of type {_attackType} cannot be charged");
    }

    public override void LaunchAutomaticAttack()
    {
        Debug.Log($"Attack of type {_attackType} is not automatic fire");
    }

    public override void LaunchStream(Transform parent)
    {
        throw new System.NotImplementedException();
    }
}
