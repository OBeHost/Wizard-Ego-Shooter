using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Scriptable Objects/Health")]
public class HealthSO : ScriptableObject
{
    [SerializeField] private float _maxHealth;

    public float _currentHealth { get; private set; }
    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }


    public void DeductHealth(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp( _currentHealth, 0f, _maxHealth);
    }

    public void AddHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth,  0f, _maxHealth);
    }
}
