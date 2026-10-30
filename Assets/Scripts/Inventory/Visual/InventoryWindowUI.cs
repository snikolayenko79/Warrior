using UnityEngine;
using Zenject;

public class InventoryWindowUI : ContainerWindowUI
{
    private IInventory _inventory;
    private IInventoryOwner  _inventoryOwner;
    
    [Inject]
    public void Construct(IInventory inventory, IGameManager gameManager)
    {
        _inventory = inventory;
        _container = inventory as IContainer;
    }
    
    protected new void Start()
    {
        base.Start();
    }
}