using UnityEngine;

public interface IAttackCharacterData
{
    float Health { get; set; }
    float MaxHealth { get; set; }
    int Experience { get; set; }
}