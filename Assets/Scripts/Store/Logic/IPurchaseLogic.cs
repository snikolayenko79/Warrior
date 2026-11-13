using UnityEngine;

public interface IPurchaseLogic
{
    bool Execute(IStore storeData, IPurchasable itemData);
}