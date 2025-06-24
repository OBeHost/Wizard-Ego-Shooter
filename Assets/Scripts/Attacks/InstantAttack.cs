using UnityEngine;
using UnityEngine.InputSystem;

public class InstantAttack : BaseAttack
{


    private void Update()
    {
        CalculateFlyTime();
    }

    public override void LaunchAttack(Vector3 direction, Transform parent = null)
    {
        Debug.Log("Instantly launched attack");
        InitializeRB();
        _rb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _hasLaunched = true;
    }
}
