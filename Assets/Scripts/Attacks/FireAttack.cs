using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FireAttack : BaseAttack
{    
    private float _flyTime = 5f;
    private bool _hasLaunched = false;

    private void Update()
    {
        if (_hasLaunched)
        {
            _flyTime -= Time.deltaTime;
            if (_flyTime <= 0 ) Destroy(gameObject);
        }
    }
    public override void LaunchAttack(Vector3 direction)
    {
        _rb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _hasLaunched = true;
    }

    private void OnCollisionEnter(Collision collision)
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
}
