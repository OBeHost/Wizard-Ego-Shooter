using UnityEngine;

public class ContinuousDamage : MonoBehaviour
{
    private float _damageInterval = 0f;
    private float _intervalReset = 0.5f;
    private float _healthDamage;

    public void Init(float intervalReset, float healthDamage)
    {
        this._intervalReset = intervalReset;
        this._healthDamage = healthDamage;
    }
    private void OnTriggerStay(Collider other)
    {
        _damageInterval += Time.fixedDeltaTime;

        if (_damageInterval >= _intervalReset)
        {
            _damageInterval = 0f;
            other.gameObject.GetComponent<IDamageable>()?.InflictDamage(_healthDamage);
        }
    }
}
