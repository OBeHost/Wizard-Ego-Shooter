using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private InputReader _reader;

    [Header("Attacking")]
    [SerializeField] private CurrentAttackHolderSO _attackHolder;
    [SerializeField] private Transform _projectileInstantiationPoint;

    private void OnEnable()
    {
        _reader.AttackEvent += Attack;
    }

    private void OnDisable()
    {
        _reader.AttackEvent -= Attack;
    }


    private void Attack()
    {
        _attackHolder.CurrentAttack.TriggerAttack(_projectileInstantiationPoint);
    }
}
