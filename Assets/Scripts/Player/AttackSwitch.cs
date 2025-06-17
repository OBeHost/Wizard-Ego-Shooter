using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSwitch : MonoBehaviour
{
    public AttackSO[] AllAttacks;
    private AttackSO[] _availableAttacks;

    [SerializeField] private InputReader _reader;

    private LinkedList<int> ints = new LinkedList<int>();

    private LinkedListNode<int> _currentNode = null;
    private int _currentIndex = 0;

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
        _availableAttacks = new AttackSO[AllAttacks.Length];

        for (int i = 0; i < AllAttacks.Length; i++)
        {
            ints.AddLast(i);
        }
        _currentNode = ints.First;
    }

    private void SwitchActive(Vector2 vec)
    {
        switch (Mathf.Sign(vec.y))
        {
            case -1:
                _currentNode = _currentNode.Previous != null ? _currentNode.Previous : ints.Last;
                break;
            case 1:
                _currentNode = _currentNode.Next != null ? _currentNode.Next : ints.First;
                break;
        }
        _currentIndex = _currentNode.Value;
        Debug.Log($"Current Index: {_currentIndex}");
    }
}
