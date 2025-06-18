using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSwitch : MonoBehaviour
{
    [SerializeField] private InputReader _reader;
    [SerializeField] private CurrentAttackHolderSO _attackHolder;

    private LinkedListNode<AttackSO> _currentNode;
    private void OnEnable()
    {
        _reader.ScrollEvent += SwitchActive;
    }

    private void OnDisable()
    {
        _reader.ScrollEvent -= SwitchActive;
    }

    private void Start()
    {
        
        //Initialize current node
        _currentNode = new LinkedListNode<AttackSO>(_attackHolder.CurrentAttack);
    }

    private void SwitchActive(Vector2 vec)
    {
        //Check if user scrolls up or down and switch current node to prev / next 
        switch (Mathf.Sign(vec.y))
        {
            case -1:
                _currentNode = _attackHolder.CurrentNode.Previous != null ? _attackHolder.CurrentNode.Previous : _attackHolder.AttacksList.Last;
                break;
            case 1:
                _currentNode = _attackHolder.CurrentNode.Next != null ? _attackHolder.CurrentNode.Next : _attackHolder.AttacksList.First;
                break;
        }
        _attackHolder.SetCurrentAttack(_currentNode);
    }
}
