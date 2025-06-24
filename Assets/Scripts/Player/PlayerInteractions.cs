using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private InputReader _reader;
    [SerializeField] private PlayerWorldInfo _playerWorldInfo;
    [SerializeField] private Transform _cameraOrientation;

    [Header("Attacking")]
    [SerializeField] private AbilityHolderSO _abilityHolder;
    [SerializeField] private Transform _projectileInstantiationPoint;

    private Vector3 _lookDirection;

   
    private bool _automaticActive = false;

    private void OnEnable()
    {
        _reader.AttackEvent += Attack;
    }

    private void OnDisable()
    {
        _reader.AttackEvent -= Attack;
    }

    private void Awake()
    {
        _playerWorldInfo.UpdatePlayerTransform(_projectileInstantiationPoint);
    }

    private void Update()
    {
        _lookDirection = _cameraOrientation.forward;
        _playerWorldInfo.UpdatePlayerOrientation(_lookDirection);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        AbilityBaseSO ability = _abilityHolder.CurrentAbility;

        ability.TriggerAbility(context);
    }
}
