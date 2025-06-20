using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticAttack : BaseAttack
{
    public override void LaunchAutomaticAttack()
    {
        Debug.Log("Automatically firing attack");
    }

    public override void CancleAttack()
    {
        Debug.Log("Canceling automatic attack");
    }

    public override void ChargeAttack(Transform parent)
    {
        Debug.Log($"Attack of type {_attackType} cannot be charged");
    }

    public override void LaunchAttack(Vector3 direction)
    {
        Debug.Log($"Attack of type {_attackType} cannot be instantly launched");
    }

    public override void LaunchStream(Transform parent)
    {
        throw new System.NotImplementedException();
    }
}
