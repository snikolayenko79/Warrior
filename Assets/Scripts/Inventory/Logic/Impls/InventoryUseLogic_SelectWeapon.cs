using UnityEditor.Searcher;
using UnityEngine;
using Zenject;

public class InventoryUseLogic_SelectWeapon : InventoryUseLogic
{
    public WeaponData weapon;
    public UIView TextView;

    private IAttackController _attackController;
    
    private IUIManager _uiManager;

    [Inject]
    public void Construct(IAttackController attackController, IUIManager uiManager)
    {
        _attackController = attackController;
        _uiManager  = uiManager;
    }
    
    public override bool Execute(IInventory inventoryData, IInventoryItem itemData)
    {
        _attackController.SelectWeapon(inventoryData.Owner as IAttacker, weapon);
        
        if (inventoryData.Owner is IMovable movable)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(movable.Position);
            _uiManager.CreateView(TextView.gameObject, weapon.weaponName + " selected", screenPos.x, screenPos.y);
        }

        return true;
    }
}