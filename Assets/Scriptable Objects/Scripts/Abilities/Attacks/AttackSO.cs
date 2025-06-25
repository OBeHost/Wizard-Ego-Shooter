using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Scriptable Objects/AttackSO")]
public class AttackSO : AbilityBaseSO
{
    [SerializeField] private GameObject _attackPrefab;

    [SerializeField] private PlayerWorldInfo _playerWorldInfo;

    private GameObject _attackInstance;

    [Header("Values for initializing attack parameters")]
    [SerializeField] public AttackType AttackType;
    [SerializeField] private float _healthDamage;
    [SerializeField] private bool _instantDamage;
    [SerializeField] private float _healthDamageDuration;
    [SerializeField] private float _damageRadius = 1f;

    [SerializeField] private bool _doesSpeedDamage;
    [SerializeField] private float _speedDamage;
    [SerializeField] private float _speedDamageDuration;

    [SerializeField] private float _launchSpeed;
    [SerializeField] private float _flyTime;
    [SerializeField] private bool _useGravity;
    [SerializeField] private float _intervalreset = 0.5f;
    [SerializeField] private float _chargeThreshold = 1f;
    [SerializeField] private float _rapidFireSpeed = 0.1f;

    private bool _automaticActive = false;
    private Rigidbody _attackRb;
    private float _chargingTimer;

    public AttackSO()
    {
        AbilityName = "";
    }

    public override void OnStarted()
    {
        Transform instPoint = _playerWorldInfo.ProjectileInstantiationPoint;
        Vector3 playerLookDir = _playerWorldInfo.PlayerCamOrientation;
        switch (AttackType)
        {
            case AttackType.SingleShot:
                InstantiateProjectile(instPoint);
                LaunchProjectile(playerLookDir);
                break;
            case AttackType.Charged:
                Charge(instPoint);
                break;
            case AttackType.Continuous:
                LaunchContinuous(instPoint);
                break;
            case AttackType.RapidFire:
                _automaticActive = true;
                CoroutineRunner.Instance.RunCoroutine(FireAutomatic(_playerWorldInfo.ProjectileInstantiationPoint));
                break;
        }
    }
    public override void OnPerformed()
    {
        switch (AttackType)
        {
            case AttackType.SingleShot:
                break;
            case AttackType.Charged:
                break;
            case AttackType.Continuous:
                break;
            case AttackType.RapidFire:
                break;
        }
    }

    public override void OnCanceled()
    {
        Transform instPoint = _playerWorldInfo.ProjectileInstantiationPoint;
        Vector3 playerLookDir = _playerWorldInfo.PlayerCamOrientation;
        switch (AttackType)
        {
            case AttackType.SingleShot:
                break;
            case AttackType.Charged:
                bool isCharged = CheckIfCharged();
                if (isCharged)
                {
                    InitializeRB();
                    LaunchProjectile(playerLookDir);
                }
                break;
            case AttackType.Continuous:
                CancelContinuous();
                break;
            case AttackType.RapidFire:
                _automaticActive = false;
                break;
        }
    }

    private void InitializeRB()
    {
        if (_attackInstance == null) return;

        _attackInstance.AddComponent<Rigidbody>();
        _attackRb = _attackInstance.GetComponent<Rigidbody>();
        _attackRb.useGravity = _useGravity;
    }

    private void InstantiateProjectile(Transform instPoint)
    {
        _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        InitializeRB();
    }

    private void LaunchProjectile(Vector3 direction)
    {
        if (_attackInstance == null) return;
        _attackInstance.transform.parent = null;
        ImpactDamage explodeComponent = _attackInstance.AddComponent<ImpactDamage>();
        explodeComponent.Init(_damageRadius, 
                              _healthDamage, 
                              _flyTime, 
                              _instantDamage, 
                              _healthDamageDuration, 
                              true);
        _attackRb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _attackInstance = null;
    }

    private void LaunchContinuous(Transform instPoint)
    {
        if (_attackInstance != null)
        {
            _attackInstance.gameObject.SetActive(true);
            return;
        }
        _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        ContinuousDamage continuousComponent = _attackInstance.AddComponent<ContinuousDamage>();
        continuousComponent.Init(_intervalreset, _healthDamage);

        if (_attackInstance.transform.parent == null)
        {
            _attackInstance.transform.parent = instPoint;
        }
        _attackInstance.gameObject.SetActive(true);
    }

    private void CancelContinuous()
    {
        if (_attackInstance == null) return;
        _attackInstance.gameObject.SetActive(false);
    }

    private void Charge(Transform instPoint)
    {
        if (_attackInstance == null)
        {
            _attackInstance = Instantiate(_attackPrefab, instPoint.position, instPoint.rotation);
        } 

        _attackInstance.gameObject.SetActive(true);
        _attackInstance.transform.SetParent(instPoint);
        _chargingTimer = Time.time;

    }

    private bool CheckIfCharged()
    {
        _chargingTimer = Time.time - _chargingTimer;
        if (_chargingTimer >= _chargeThreshold) return true;

        _attackInstance?.gameObject.SetActive(false);
        return false;
    }

    private IEnumerator FireAutomatic(Transform instPoint)
    {
        while (_automaticActive)
        {
            InstantiateProjectile(_playerWorldInfo.ProjectileInstantiationPoint);
            LaunchProjectile(_playerWorldInfo.PlayerCamOrientation);
            yield return new WaitForSeconds(_rapidFireSpeed);
        }
    }
}

public enum AttackType
{
    SingleShot,
    Charged,
    Continuous, 
    RapidFire
}
