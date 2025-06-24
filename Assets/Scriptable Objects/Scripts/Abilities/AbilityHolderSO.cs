using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityHolderSO", menuName = "Scriptable Objects/AbilityHolderSO")]
public class AbilityHolderSO : ScriptableObject
{
    [SerializeField] private InputReader _reader;

    //In order to assign the SO instances in the editor 
    [SerializeField] private AbilityBaseSO[] _allAbilities;




    public AbilityBaseSO CurrentAbility { get; private set; }

    private LinkedListNode<AbilityBaseSO> _currentNode;
    private int _maxIndex;

    private LinkedList<AbilityBaseSO> _abilitiesList;
    private Queue<AbilityBaseSO> _allAbilitiesQueue = new Queue<AbilityBaseSO>();


    private Dictionary<int, LinkedListNode<AbilityBaseSO>> _abilitiesDict;

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
        foreach (var ability in _allAbilities)
        {
            _allAbilitiesQueue.Enqueue(ability);
        }

        _currentNode = new LinkedListNode<AbilityBaseSO>(_allAbilitiesQueue.Dequeue());
        CurrentAbility = _currentNode.Value;

        _abilitiesList = new LinkedList<AbilityBaseSO>();
        _abilitiesList.AddLast(_currentNode);

        _abilitiesDict = new Dictionary<int, LinkedListNode<AbilityBaseSO>>();
        _maxIndex = 1;
        _abilitiesDict[_maxIndex] = _currentNode;
    }

    public void SetCurrentAttack(LinkedListNode<AbilityBaseSO> attack)
    {
        CurrentAbility = attack.Value;
    }

    private void ScrollSwitch(Vector2 vec)
    {
        //Check if user scrolls up or down and switch current node to prev / next 
        switch (Mathf.Sign(vec.y))
        {
            case -1:
                _currentNode = _currentNode.Previous != null ? _currentNode.Previous : _abilitiesList.Last;
                break;
            case 1:
                _currentNode = _currentNode.Next != null ? _currentNode.Next : _abilitiesList.First;
                break;
        }
        SetCurrentAttack(_currentNode);
    }

    private void KeySwitch(int index)
    {
        if (!_abilitiesDict.ContainsKey(index)) return;

        _currentNode = _abilitiesDict[index];
        SetCurrentAttack(_currentNode);
    }

    private void UnlockAttack()
    {
        if (_allAbilitiesQueue.Count == 0) return;

        //Add the attack to the linked list for scrolling
        LinkedListNode<AbilityBaseSO> newNode = new LinkedListNode<AbilityBaseSO>(_allAbilitiesQueue.Dequeue());
        _abilitiesList.AddLast(newNode);

        //Add the attack to the dictionary for key access
        _maxIndex++;
        _abilitiesDict[_maxIndex] = newNode;
    }

}
