using TMPro;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private TextMeshProUGUI _healthDisplay;
    private ObjectHealth _enemyHealth;

    private void Start()
    {
        _enemyHealth = this.gameObject.GetOrAdd<ObjectHealth>();
    }

    private void Update()
    {
        _healthDisplay.text = _enemyHealth._currentHealth.ToString();
    }

    public void Heal(float amount)
    {
        _enemyHealth.AddHealth(amount);
    }

    public void InflictDamage(float amount)
    {
        _enemyHealth.DeductHealth(amount);
    }
}
