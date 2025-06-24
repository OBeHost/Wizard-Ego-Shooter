using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticAttack : BaseAttack
{

    private bool _automaticActive = false;


    private void Update()
    {
        CalculateFlyTime();
    }

    public override void LaunchAttack(Vector3 direction, Transform parent = null)
    {
        InitializeRB();
        _rb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _hasLaunched = true;
    }
}
