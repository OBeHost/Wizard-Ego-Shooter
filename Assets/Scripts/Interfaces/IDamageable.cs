using UnityEngine;

public interface IDamageable
{
    public void InflictDamage(float amount);
    public void Heal(float amount);

    public void SetReceivingLastingDamage(bool isReceiving);
    public bool GetReceivingLastingDamage();
}
