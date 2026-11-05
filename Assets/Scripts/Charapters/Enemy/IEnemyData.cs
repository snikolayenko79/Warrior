using UnityEngine;

public interface IEnemyData
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; }
    int Experience { get; set; }
    void ResetData();
}