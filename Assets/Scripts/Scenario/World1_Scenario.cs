using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class World1_Scenario : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public Transform enemy1SpawnPoint;
    public SpecifiedPath enemy1Path;
    
    public GameObject enemy2Prefab;
    public Transform enemy2SpawnPoint;
    public SpecifiedPath enemy2Path;
    
    public GameObject villagerPrefab;
    public Transform villagerSpawnPoint;
    public SpecifiedPath villagerPath;
    
    public GameObject sellerPrefab;
    public Transform sellerSpawnPoint;
    public SpecifiedPath sellerPath;
    
    public CharacterFactory characterFactory;
    
    public CharacterStateController characterStateController;
    [FormerlySerializedAs("characterStatePlayerInput")] public BehaviourBase playerInput;
    [FormerlySerializedAs("characterPathMove")] [FormerlySerializedAs("characterStatePathMove")] public BehaviourFollowPath followPath;
    public BehaviourAttackPlayer attackPlayer;
    [FormerlySerializedAs("startFollowPlayer")] public BehaviourFollowPlayer followPlayer;
    public Player player = null;
    
    private IDialogManager _dialogManager = null;

    public Dialogue DialogueEntrance = null;
    
    [Inject]
    public void Construct(IDialogManager dialogManager)
    {
        _dialogManager = dialogManager;
    }
    
    void Start()
    {
        StartCoroutine(World1_Scenario_Coroutine());
    }

    private IEnumerator World1_Scenario_Coroutine()
    {
        yield return new WaitForSeconds(1);
        
        _dialogManager.StartDialogue(DialogueEntrance);
        
        while (_dialogManager.IsDialogueRunning)
            yield return null;

        characterStateController.RegisterEntity(player);
        characterStateController.StateStart(player, playerInput);
        
        GameObject enemy1 = characterFactory.SpawnCharapter(enemy1Prefab, enemy1SpawnPoint.position, enemy1SpawnPoint.rotation * enemy1Prefab.transform.rotation);
        Character character1 = enemy1.GetComponent<Character>();
        character1.CurrentPath = new RandomPath(character1);
        characterStateController.StateStart(character1, followPath);
        characterStateController.StateStart(character1, followPlayer);
        characterStateController.StateStart(character1, attackPlayer);
        
        while (!character1.IsDead)
            yield return null;
        
        GameObject enemy2 = characterFactory.SpawnCharapter(enemy2Prefab, enemy2SpawnPoint.position, enemy2SpawnPoint.rotation * enemy2Prefab.transform.rotation);
        Character character2 = enemy2.GetComponent<Character>();
        character2.CurrentPath = enemy2Path;
        characterStateController.StateStart(character2, followPath);
        
        while (!character2.IsDead)
            yield return null;

        yield return new WaitForSeconds(5);
        
        GameObject villager = characterFactory.SpawnCharapter(villagerPrefab, villagerSpawnPoint.position, villagerSpawnPoint.rotation * villagerPrefab.transform.rotation);
        Character characterVillager = villager.GetComponent<Character>();
        characterVillager.CurrentPath = villagerPath;
        characterStateController.StateStart(characterVillager, followPath);
        
        yield return null;
        while (characterVillager.IsMoving)
            yield return null;
        
        GameObject seller = characterFactory.SpawnCharapter(sellerPrefab, sellerSpawnPoint.position, sellerSpawnPoint.rotation * sellerPrefab.transform.rotation);
        Character characterSeller = seller.GetComponent<Character>();
        characterSeller.CurrentPath = sellerPath;
        characterStateController.StateStart(characterSeller, followPath);
        
        yield return null;
        while (characterSeller.IsMoving)
            yield return null;
    }
}
