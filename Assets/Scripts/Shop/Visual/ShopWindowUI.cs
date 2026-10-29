using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ShopWindowUI : ContainerWindowUI
{
    protected override void OnSlotUpdate(ContainerSlotUI slot)
    {
        base.OnSlotUpdate(slot);
        
        if (_interactor is ICustomer customer && slot.Item is IPurchasable purchasable)
            slot.SetAccessibility(customer.Coins >= purchasable.Price);
    }
}
