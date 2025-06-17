using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthSO _playerHealth;

    public void Heal(float amount)
    {
        _playerHealth.AddHealth(amount);
    }

    public void InflictDamage(float amount)
    {
        _playerHealth.DeductHealth(amount);
    }

    
}
