using UnityEngine;
using Zenject;
using Screen = UnityEngine.Device.Screen;

public class PurchaseLogic_AddToInventory : PurchaseLogic
{
    public InventoryItem inventoryItem;
    public UIView TextView;
    
    private IInventory _inventory;
    
    private IUIManager _uiManager;
    
    [Inject]
    public void Construct(IInventory inventory, IUIManager uiManager)
    {
        _inventory = inventory;
        _uiManager  = uiManager;
    }
    
    public override void Execute(IStore storeData, IPurchasable itemData)
    {
        if (!inventoryItem.IsStackable && _inventory.ContainsItem(inventoryItem))
        {
            _uiManager.CreateView(TextView.gameObject, "You already have this item!", (float)Screen.width/2.0f, (float)Screen.height*0.95f);
            return;
        }
        
        if (_inventory.ItemsCount >= _inventory.MaxItemsCount)
        {
            _uiManager.CreateView(TextView.gameObject, "Your inventory is full!", (float)Screen.width/2.0f, (float)Screen.height*0.95f);
            return;
        }

        _inventory.AddItemToInventory(inventoryItem);
        _uiManager.CreateView(TextView.gameObject, "+ " + inventoryItem.Name, (float)Screen.width/2.0f, (float)Screen.height*0.95f);
    }
}