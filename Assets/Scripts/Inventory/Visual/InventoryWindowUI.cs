using UnityEngine;
using Zenject;

public class InventoryWindowUI : ContainerWindowUI
{
    private IInventory _inventory;
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
        _container = inventory as IContainer;
    }
}