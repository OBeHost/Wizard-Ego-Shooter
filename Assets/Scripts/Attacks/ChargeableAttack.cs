using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeableAttack : BaseAttack
{
    private float _releaseValue = 1f;
    private float _currentCharge = 0f;

    private bool _isCharging = false;
    private bool _isCharged = false;

    private void Update()
    {
        if (_isCharging)
        {
            _currentCharge += Time.deltaTime;
        }   

        if (_currentCharge >= _releaseValue)
        {
            _isCharged = true;
        }

        CalculateFlyTime();
    }

    public void ChargeAttack(Transform parent)
    {
        Debug.Log("Charging attack");
        this.gameObject.SetActive(true);
        this.transform.SetParent(parent);
        _isCharging = true;

    }

    public void CancleAttack()
    {
        Debug.Log("Canceled because attack was not charged enough");
        this.gameObject.SetActive(false);
        _currentCharge = 0f;
    }


    public override void LaunchAttack(Vector3 direction, Transform parent = null)
    {
        _isCharging = false;
        if (!_isCharged)
        {
            CancleAttack();
            return;
        }
        this.transform.parent = null;
        InitializeRB();
        _rb.AddForce(direction * _launchSpeed * 100f, ForceMode.Force);
        _hasLaunched = true;
    }


}
