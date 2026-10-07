using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World1_Scenario : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public Transform enemy1SpawnPoint;
    public SpecifiedPath enemy1Path;
    
    public GameObject enemy2Prefab;
    public Transform enemy2SpawnPoint;
    public SpecifiedPath enemy2Path;
    
    public GameObject villagerPrefab;
    public GameObject sellerPrefab;
    public CharacterFactory characterFactory;
    
    void Start()
    {
        StartCoroutine(World1_Scenario_Coroutine());
    }

    private IEnumerator World1_Scenario_Coroutine()
    {
        yield return null;

        GameObject enemy1 = characterFactory.SpawnCharapter(enemy1Prefab, enemy1SpawnPoint.position, enemy1SpawnPoint.rotation * enemy1Prefab.transform.rotation);
        Character character1 = enemy1.GetComponent<Character>();
        character1.CurrentPath = enemy1Path;

        while (!character1.IsDead)
            yield return null;
        
        GameObject enemy2 = characterFactory.SpawnCharapter(enemy2Prefab, enemy2SpawnPoint.position, enemy2SpawnPoint.rotation * enemy2Prefab.transform.rotation);
        Character character2 = enemy2.GetComponent<Character>();
        character2.CurrentPath = enemy2Path;
        
        while (!character2.IsDead)
            yield return null;
    }
}
