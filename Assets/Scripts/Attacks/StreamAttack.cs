using UnityEngine;
using UnityEngine.InputSystem;

public class StreamAttack : BaseAttack
{

    private float _damageInterval = 0f;
    private float _intervalReset = 0.5f;

    private void OnTriggerStay(Collider other)
    {
        _damageInterval += Time.fixedDeltaTime;

        if (_damageInterval >= _intervalReset)
        {
            _damageInterval = 0f;
            other.gameObject.GetComponent<IDamageable>()?.InflictDamage(_healthDamage);
        }
    }

    public override void LaunchStream(Transform parent)
    {
        Debug.Log("Extending stream collider");
        if (this.transform.parent == null)
        {
            this.transform.parent = parent;
        }
        this.gameObject.SetActive(true);
    }
    public override void CancleAttack()
    {
        Debug.Log("Taking back stream collider");
        this.gameObject.SetActive(false);
    }

    public override void LaunchAttack(Vector3 direction)
    {

    }



    public override void ChargeAttack(Transform parent)
    {
        Debug.Log($"Attack of type {_attackType} cannot be charged");
    }

    public override void LaunchAutomaticAttack()
    {
        Debug.Log($"Attack of type {_attackType} is not automatic fire");
    }

}
