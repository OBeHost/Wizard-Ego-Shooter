using UnityEngine;

[CreateAssetMenu(fileName = "AbilityBaseSO", menuName = "Scriptable Objects/AbilityBaseSO")]
public class AbilityBaseSO : ScriptableObject, IAbility
{
    public string AbilityName;
}
