using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentAttackHolderSO", menuName = "Scriptable Objects/CurrentAttackHolderSO")]
public class CurrentAttackHolderSO : ScriptableObject
{
    [SerializeField] private InputReader _reader;

    //In order to assign the SO instances in the editor 
    [SerializeField] private AttackSO[] AllAttacks;



    public LinkedListNode<AttackSO> CurrentNode { get; private set; }
    public LinkedList<AttackSO> AttacksList { get; private set; }
    public AttackSO CurrentAttack {  get; private set; }


    private Queue<AttackSO> _allAttacksQueue = new Queue<AttackSO>();

    private void OnEnable()
    {
        _reader.UnlockEvent += UnlockAttack;

        foreach (var attack in AllAttacks)
        {
            _allAttacksQueue.Enqueue(attack);
        }

        CurrentNode = new LinkedListNode<AttackSO>(_allAttacksQueue.Dequeue());
        CurrentAttack = CurrentNode.Value;

        AttacksList = new LinkedList<AttackSO>();
        AttacksList.AddLast(CurrentNode);
    }

    private void OnDisable()
    {
        _reader.UnlockEvent -= UnlockAttack;
    }

    public void SetCurrentAttack(LinkedListNode<AttackSO> attack)
    {
        CurrentNode = attack;
        CurrentAttack = attack.Value;
    }

    private void UnlockAttack()
    {
        if (_allAttacksQueue.Count == 0) return;

        LinkedListNode<AttackSO> newNode = new LinkedListNode<AttackSO>(_allAttacksQueue.Dequeue());
        AttacksList.AddLast(newNode);

        foreach (var attack in AttacksList)
        {
            Debug.Log($"Attack: {attack}");
        }
    }
}
