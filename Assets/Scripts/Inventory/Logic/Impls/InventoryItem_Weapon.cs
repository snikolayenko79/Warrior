using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "InventoryItem_Weapon", menuName = "Inventory/Weapon item")]
public class InventoryItem_Weapon : InventoryItem
{
    public WeaponData weapon;
    public override void OnUse(IInventoryOwner inventoryOwner)
    {
        if (inventoryOwner is IAttacker attacker)
        {
            // TODO.
            GameObject.FindFirstObjectByType<AttackController>().SelectWeapon(attacker, weapon);
        }
    }
}