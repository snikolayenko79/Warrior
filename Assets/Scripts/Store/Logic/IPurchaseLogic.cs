using UnityEngine;

public interface IPurchaseLogic
{
    void Execute(IStore storeData, IPurchasable itemData);
}