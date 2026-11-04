using UnityEngine;
using Zenject;

public class InventoryUseLogic_Heal : InventoryUseLogic
{
    public float healthAmount;
    public UIView TextView;
    
    private IUIManager _uiManager;

    [Inject]
    public void Construct(IAttackController attackController, IUIManager uiManager)
    {
        _uiManager  = uiManager;
    }
    
    public override void Execute(IInventory inventoryData, IInventoryItem itemData)
    {
        if (inventoryData.Owner is IDamageable damageable)
        {
            float healthBefore = damageable.Health;
            damageable.Health += healthAmount;

            float heal = (damageable.Health - healthBefore);
            if (heal > 0)
            {
                if (inventoryData.Owner is IMovable movable)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(movable.Position);
                    _uiManager.CreateView(TextView.gameObject,
                        "+" + (damageable.Health - healthBefore).ToString() + " health points", screenPos.x,
                        screenPos.y);
                }
            }
        }
    }
}