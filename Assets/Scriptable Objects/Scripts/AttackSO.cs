using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : ScriptableObject
{
    public string AttackName;

    public bool unlocked;

    public void TriggerAttack()
    {

    }
}
