using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    #region Current Ability Display
    [SerializeField] private TextMeshProUGUI _currentAttackDisplay;
    [SerializeField] private AbilityHolderSO _abilityHolder;
    #endregion


    private void Update()
    {
        _currentAttackDisplay.text = $"Current Attack: {_abilityHolder.CurrentAbility.AbilityName}";
    }

}
