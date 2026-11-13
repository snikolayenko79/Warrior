using UnityEngine;
using System.Collections.Generic;

public class Enemy : AttackCharacter
{    
    [SerializeField]private EnemyData enemyData;

    protected new void Awake()
    {
        base.Awake();
        
        if (!enemyData)
            Debug.LogError($"[Enemy] На объекте {name} отсутствует обязательный компонент данных EnemyData!", this);
    }
    
    public override void InitializeData()
    {
        CharacterData = enemyData;
        base.InitializeData();
    }
}