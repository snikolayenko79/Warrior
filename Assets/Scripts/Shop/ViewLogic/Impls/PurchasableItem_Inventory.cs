using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Screen = UnityEngine.Device.Screen;

public class PurchasableItem_Inventory : PurchasableView
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
    
    public override void OnPurchase(IStore storeData, IPurchasable itemData)
    {
        if (_inventory.ContainsItem(inventoryItem))
        {
            _uiManager.CreateView(TextView.gameObject, "You already have this item", (float)Screen.width/2.0f, (float)Screen.height/2.0f);
            return;
        }

        _inventory.AddItemToInventory(inventoryItem);
        _uiManager.CreateView(TextView.gameObject, "Item added to inventory", (float)Screen.width/2.0f, (float)Screen.height/2.0f);
    }
}