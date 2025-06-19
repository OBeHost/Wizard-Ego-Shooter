using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentAttackHolderSO", menuName = "Scriptable Objects/CurrentAttackHolderSO")]
public class CurrentAttackHolderSO : ScriptableObject
{
    [SerializeField] private InputReader _reader;

    //In order to assign the SO instances in the editor 
    [SerializeField] private AttackSO[] _allAttacks;


    public AttackSO CurrentAttack { get; private set; }

    private LinkedListNode<AttackSO> _currentNode;
    private int _maxIndex;

    private LinkedList<AttackSO> _attacksList;
    private Queue<AttackSO> _allAttacksQueue = new Queue<AttackSO>();


    private Dictionary<int, LinkedListNode<AttackSO>> _attacksDict;

    private void OnEnable()
    {
        _reader.UnlockEvent += UnlockAttack;
        _reader.ScrollEvent += ScrollSwitch;
        _reader.KeySwitchEvent += KeySwitch;

        Initialize();
    }

    private void OnDisable()
    {
        _reader.UnlockEvent -= UnlockAttack;
        _reader.ScrollEvent -= ScrollSwitch;
        _reader.KeySwitchEvent -= KeySwitch;
    }

    private void Initialize()
    {
        foreach (var attack in _allAttacks)
        {
            _allAttacksQueue.Enqueue(attack);
        }

        _currentNode = new LinkedListNode<AttackSO>(_allAttacksQueue.Dequeue());
        CurrentAttack = _currentNode.Value;

        _attacksList = new LinkedList<AttackSO>();
        _attacksList.AddLast(_currentNode);

        _attacksDict = new Dictionary<int, LinkedListNode<AttackSO>>();
        _maxIndex = 1;
        _attacksDict[_maxIndex] = _currentNode;
    }

    public void SetCurrentAttack(LinkedListNode<AttackSO> attack)
    {
        CurrentAttack = attack.Value;
    }

    private void ScrollSwitch(Vector2 vec)
    {
        //Check if user scrolls up or down and switch current node to prev / next 
        switch (Mathf.Sign(vec.y))
        {
            case -1:
                _currentNode = _currentNode.Previous != null ? _currentNode.Previous : _attacksList.Last;
                break;
            case 1:
                _currentNode = _currentNode.Next != null ? _currentNode.Next : _attacksList.First;
                break;
        }
        SetCurrentAttack(_currentNode);
    }

    private void KeySwitch(int index)
    {
        if (!_attacksDict.ContainsKey(index)) return;

        _currentNode = _attacksDict[index];
        SetCurrentAttack(_currentNode);
    }

    private void UnlockAttack()
    {
        if (_allAttacksQueue.Count == 0) return;

        //Add the attack to the linked list for scrolling
        LinkedListNode<AttackSO> newNode = new LinkedListNode<AttackSO>(_allAttacksQueue.Dequeue());
        _attacksList.AddLast(newNode);

        //Add the attack to the dictionary for key access
        _maxIndex++;
        _attacksDict[_maxIndex] = newNode;
    }
}
