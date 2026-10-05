using UnityEngine;

public interface IWeaponBehaviour
{
    void Execute(IAttacker caster, OnWeaponReadyToDamage  onWeaponReadyToDamage);
    bool IsEnded { get; }
}

public delegate void OnWeaponReadyToDamage(Collider damageZone);

public abstract class WeaponBehaviour : MonoBehaviour, IWeaponBehaviour
{
    public abstract void Execute(IAttacker caster, OnWeaponReadyToDamage  onWeaponReadyToDamage);
    public abstract bool IsEnded { get; }
}