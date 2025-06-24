using UnityEngine;
using UnityEngine.InputSystem;

public class StreamAttack : BaseAttack
{

    private float _damageInterval = 0f;
    private float _intervalReset = 0.5f;

    new private void OnTriggerEnter(Collider other)
    {
        //Needs to shadow OnTriggerEnter of BaseAttack because this 
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
    public void CancleAttack()
    {
        Debug.Log("Taking back stream collider");
        this.gameObject.SetActive(false);
    }

    public override void LaunchAttack(Vector3 direction = default, Transform parent = null)
    {
        Debug.Log("Extending stream collider");
        if (this.transform.parent == null)
        {
            this.transform.parent = parent;
        }
        this.gameObject.SetActive(true);
    }
}
