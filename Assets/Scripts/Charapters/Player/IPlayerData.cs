using UnityEngine;

public interface IPlayerData
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; }
    int Coins { get; set; }
    int Experience { get; set; }
    void ResetData();
}