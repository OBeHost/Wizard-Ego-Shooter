using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeableAttack : BaseAttack
{
    private float _releaseValue = 1f;
    private float _currentCharge = 0f;

    private bool _isCharging = false;
    private bool _isCharged = false;

    private bool _hasLaunched = false;
    private float _flyTime = 5f;

    private void Update()
    {
        if (_isCharging)
        {
            _currentCharge += Time.deltaTime;
        }   

        if (_currentCharge >= _releaseValue)
        {
            _isCharged = true;
        }

        if (_hasLaunched)
        {
            _flyTime -= Time.deltaTime;
            if (_flyTime <= 0) Destroy(gameObject);
        }
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

    public override void ChargeAttack(Transform parent)
    {
        Debug.Log("Charging attack");
        this.gameObject.SetActive(true);
        this.transform.SetParent(parent);
        _isCharging = true;

    }

    public override void CancleAttack()
    {
        Debug.Log("Canceled because attack was not charged enough");
        this.gameObject.SetActive(false);
        _currentCharge = 0f;
    }


    public override void LaunchAttack(Vector3 direction)
    {
        _isCharging = false;
        if (!_isCharged)
        {
            CancleAttack();
            return;
        }
        this.transform.parent = null;
        InitializeRB();
        _rb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _hasLaunched = true;
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
