using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "AbilityBaseSO", menuName = "Scriptable Objects/AbilityBaseSO")]
public abstract class AbilityBaseSO : ScriptableObject
{
    public string AbilityName;

    public abstract void TriggerAbility(InputAction.CallbackContext context);

    public abstract void OnStarted();
    public abstract void OnPerformed();
    public abstract void OnCanceled();
}
