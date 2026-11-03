using UnityEngine;
using Zenject;

public class InventoryItem_Weapon : InventoryItemView
{
    public WeaponData weapon;

    private IAttackController _attackController;

    [Inject]
    public void Construct(IAttackController attackController)
    {
        _attackController = attackController;
    }

    public override void OnUse(IInventory inventoryData, IInventoryItem itemData)
    {
        _attackController.SelectWeapon(inventoryData.Owner as IAttacker, weapon);
    }
}