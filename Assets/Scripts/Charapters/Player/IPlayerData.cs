using UnityEngine;

public interface IPlayerData : IData
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; }
    int Coins { get; set; }
    int Experience { get; set; }
}