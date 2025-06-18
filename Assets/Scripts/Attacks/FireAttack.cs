using UnityEngine;

public class FireAttack : BaseAttack
{
    private Rigidbody _rb;
    private bool _canMove = false;

    public override void InitializeAttack()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * 600f, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
