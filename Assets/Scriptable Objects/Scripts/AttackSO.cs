using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public string AttackName;
    public AttackSO()
    {
        AttackName = "";
    }

    public void TriggerAttack()
    {

    }
}
