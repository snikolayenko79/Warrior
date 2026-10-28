using UnityEngine;

public interface ICustomer
{
    int Coins { get; set; }
    
    bool CanAfford(int amount);
    void DeductCoins(int amount);
}