using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class World1_Scenario : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public SpecifiedPath playerPath1;
    
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
    
    private IDialogManager _dialogManager = null;

    public Dialogue DialogueEntrance = null;
    public Dialogue DialogueYodan1 = null;

    private BehaviourAttackPlayer _behaviourAttackPlayer;
    private BehaviourFollowPlayer _behaviourFollowPlayer;
    
    [Inject]
    public void Construct(
        IDialogManager dialogManager, 
        BehaviourAttackPlayer behaviourAttackPlayer,
        BehaviourFollowPlayer behaviourFollowPlayer)
    {
        _dialogManager = dialogManager;
        _behaviourAttackPlayer = behaviourAttackPlayer;
        _behaviourFollowPlayer = behaviourFollowPlayer;
    }
    
    void Start()
    {
        StartCoroutine(World1_Scenario_Coroutine());
    }

    private IEnumerator World1_Scenario_Coroutine()
    {
        GameObject goPlayer = characterFactory.SpawnCharapter(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation * playerPrefab.transform.rotation);
        Character chPlayer = goPlayer.GetComponent<Character>();
        chPlayer.CurrentPath = playerPath1;
        
        characterStateController.StateStart(chPlayer, followPath);
        
        // TODO. Как минимум отказаться от добавления прямо в список.
        _behaviourAttackPlayer.players.Add(chPlayer as Player);
        _behaviourFollowPlayer.players.Add(chPlayer as Player);
        
        yield return null;
        
        while (chPlayer.IsMoving)
            yield return null;
        
        characterStateController.StateEnd(chPlayer, followPath);
        
        yield return new WaitForSeconds(1.0f);
        
        _dialogManager.StartDialogue(DialogueEntrance);
        
        while (_dialogManager.IsDialogueRunning)
            yield return null;
        
         GameObject goYodan1 = characterFactory.SpawnCharapter(enemy1Prefab, enemy1SpawnPoint.position, enemy1SpawnPoint.rotation * enemy1Prefab.transform.rotation);
         Character chYodan1 = goYodan1.GetComponent<Character>();
         chYodan1.CurrentPath = enemy1Path;
         characterStateController.StateStart(chYodan1, followPath);
        
         yield return null;
         
         while (chYodan1.IsMoving)
             yield return null;
        
         yield return new WaitForSeconds(1.0f);
         
         _dialogManager.StartDialogue(DialogueYodan1);
        
         while (_dialogManager.IsDialogueRunning)
             yield return null;
        
         characterStateController.StateStart(chPlayer, playerInput);
         
         chYodan1.CurrentPath = new RandomPath(chYodan1);
         characterStateController.StateStart(chYodan1, followPlayer);
         characterStateController.StateStart(chYodan1, attackPlayer);
        
         GameObject goYodan2 = characterFactory.SpawnCharapter(enemy2Prefab, enemy2SpawnPoint.position, enemy2SpawnPoint.rotation * enemy2Prefab.transform.rotation);
         Character chYodan2 = goYodan2.GetComponent<Character>();
         chYodan2.CurrentPath = enemy2Path;
         characterStateController.StateStart(chYodan2, followPath);
         characterStateController.StateStart(chYodan2, followPlayer);
         characterStateController.StateStart(chYodan2, attackPlayer);
         
         while (!chYodan1.IsDead || !chYodan2.IsDead)
            yield return null;

        yield return new WaitForSeconds(3);
        
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
