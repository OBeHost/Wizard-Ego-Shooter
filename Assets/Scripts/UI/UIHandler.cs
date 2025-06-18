using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    #region Current Attack Display
    [SerializeField] private TextMeshProUGUI _currentAttackDisplay;
    [SerializeField] private CurrentAttackHolderSO _attackHolder;
    #endregion


    private void Update()
    {
        _currentAttackDisplay.text = $"Current Attack: {_attackHolder.CurrentAttack.AttackName}";
    }

}
