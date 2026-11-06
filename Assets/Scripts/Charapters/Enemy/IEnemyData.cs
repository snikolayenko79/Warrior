using UnityEngine;

public interface IEnemyData : IData
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; }
    int Experience { get; set; }
}