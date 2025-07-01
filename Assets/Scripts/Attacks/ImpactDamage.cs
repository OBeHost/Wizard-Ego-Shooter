using System.Collections;
using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
    private float _damageRadius;
    private float _healthDamage;
    private float _flyTime;

    private bool _instantDamage;
    private float _damageDuration;

    private bool _hasLaunched = false;

    private bool _impacted = false;
    private float _timer = 0f;

    private ParticleSystem _impactParticle;
    private ParticleSystem _particleInstance;
    public void Init(
                     float damageRadius,
                     float healthDamage,
                     float flyTime,
                     bool instantDamage,
                     float damageDuration,
                     bool hasLaunched,
                     ParticleSystem impactParticle)
    {
        this._damageRadius = damageRadius;
        this._healthDamage = healthDamage;
        this._flyTime = flyTime;
        this._instantDamage = instantDamage;
        this._damageDuration = damageDuration;
        this._hasLaunched = hasLaunched;
        this._impactParticle = impactParticle;
    }

    private void Update()
    {
        DestroyInFlight();
        DurationTimer();
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode(transform.position, _damageRadius);
    }

    private void Explode(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        //Make the projectile invisible and disable everything before deciding what to do
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<PlayerStats>() != null) continue;
            if (collider.GetComponent<IDamageable>() == null) continue;

            //Particles for burning enemies
            //TODO: Make particle a child of the collider and also implement basic impact particle spawning
            Vector3 particleSpawnPos = new Vector3(collider.transform.position.x, 0f, collider.transform.position.z);
            _particleInstance = Instantiate(_impactParticle, particleSpawnPos, Quaternion.identity);
            _particleInstance.Play();

            switch (_instantDamage)
            {
                case true:
                    DealInstantDamage(collider);
                    Destroy(gameObject);
                    break;
                case false:
                    DealLastingDamage(collider);
                    break;
            }
        }

        if (!_impacted)
        {
            Destroy(gameObject);
        }
    }

    private void DealInstantDamage(Collider collider)
    {
        collider.GetComponent<IDamageable>()?.InflictDamage(_healthDamage);
    }

    private void DealLastingDamage(Collider collider)
    {
        //Dealing damage upon impact no matter if the target is already receiving lasting damage
        DealInstantDamage(collider);

        //If the target is receiving lasting damage already, nothing more should happen
        if (collider.GetComponent<IDamageable>().GetReceivingLastingDamage())
        {
            Destroy(gameObject);
        }

        _impacted = true;

        //Run the coroutine for repeated damage dealing
        StartCoroutine(RepeatDamage(collider));
    }

    private IEnumerator RepeatDamage(Collider collider)
    {
        //Block other projectiles from dealing lasting damage while this coroutine runs
        collider.GetComponent<IDamageable>().SetReceivingLastingDamage(true);

        while (_impacted)
        {
            DealInstantDamage(collider);
            yield return new WaitForSeconds(1f);
        }
        //Other projectiles can now deal lasting damage to the target again
        collider.GetComponent<IDamageable>().SetReceivingLastingDamage(false);
        //Projectile is not needed anymore and can be destroyed
        Destroy(gameObject);

    }

    /// <summary>
    /// Count the time from impact
    /// </summary>
    private void DurationTimer()
    {
        if (_impacted)
        {
            _timer += Time.deltaTime;
            if (_timer >= _damageDuration)
            {
                _impacted = false;
                _timer = 0f;
            }
        }
    }

    /// <summary>
    /// If projectile doesn't anything for too long, destroy it.
    /// </summary>
    private void DestroyInFlight()
    {
        if (_hasLaunched)
        {
            _flyTime -= Time.deltaTime;
            if (_flyTime <= 0) Destroy(gameObject);
        }
    }
}
