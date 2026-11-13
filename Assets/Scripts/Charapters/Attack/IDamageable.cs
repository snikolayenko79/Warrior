using UnityEngine;

public interface IDamageable
{
    bool IsDead { get; }
    void TakeDamage(float damageAmount);
    float Health { get; set; }
    float MaxHealth { get; }
    void Dead();
}