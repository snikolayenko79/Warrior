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
    public SpecifiedPath playerPath2;
    
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
    public BehaviourBase playerInput;
    public BehaviourPlayerInteract playerInteract;
    public BehaviourFollowPath followPath;
    public BehaviourAttackPlayer attackPlayer;
    public BehaviourFollowPlayer followPlayer;
    
    private IDialogManager _dialogManager = null;

    public Dialogue DialogueEntrance = null;
    public Dialogue DialogueYodan1 = null;
    public Dialogue DialogueVillager = null;
    public Dialogue DialogueSeller = null;
    public Dialogue DialogueVillager2 = null;
    
    private IGameManager _gameManager;
    private IAttackController _attackController;
    private SpellController _spellController;
    private BehaviourAttackPlayer _behaviourAttackPlayer;
    private BehaviourFollowPlayer _behaviourFollowPlayer;

    public Camera levelCamera;
    
    public WeaponData defaultPlayerWeapon = null;
    public WeaponData defaultEnemyWeapon = null;
    
    public SpellData fireBallSpell;
    
    private IUIManager _uiManager;

    public UIView TextView;
    public ShopWindowUI ShopUI;

    private IInventory _inventory;
    public InventoryItem axe;
    
    [Inject]
    public void Construct(
        IDialogManager dialogManager,
        IGameManager gameManager,
        IAttackController attackController,
        SpellController spellController,
        BehaviourAttackPlayer behaviourAttackPlayer,
        BehaviourFollowPlayer behaviourFollowPlayer,
        IUIManager uiManager,
        IInventory inventory)
    {
        _dialogManager = dialogManager;
        _gameManager = gameManager;
        _attackController = attackController;
        _spellController = spellController;
        _behaviourAttackPlayer = behaviourAttackPlayer;
        _behaviourFollowPlayer = behaviourFollowPlayer;
        _uiManager = uiManager;
        _inventory = inventory;
    }
    
    void Start()
    {
        StartCoroutine(World1_Scenario_Coroutine());
        playerInteract.OnInteract += OnInteract;
    }

    private void OnInteract(IInteractor player, IInteractor interactor)
    {
        if (interactor.Name == "Villager")
        {
            _dialogManager.StartDialogue(DialogueVillager2);
        }
        else if (interactor.Name == "Seller")
        {
            _dialogManager.StartDialogue(DialogueSeller);
        }
    }

    private IEnumerator World1_Scenario_Coroutine()
    {
        // TODO. Убрать.
        yield return new WaitForSeconds(3);
        
        GameObject goPlayer = _gameManager.CreateCharacter("Warrior", playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation * playerPrefab.transform.rotation, SideType.Player);
        
        Character chPlayer = goPlayer.GetComponent<Character>();
        
        _inventory.AddItemToInventory(axe);

        chPlayer.CurrentPath = playerPath1;
        characterStateController.StateStart(chPlayer, followPath);
        
        Player player = chPlayer as Player;
        _attackController.SelectWeapon(player, defaultPlayerWeapon);
        
        // TODO. Как минимум отказаться от добавления прямо в список.
        _behaviourAttackPlayer.players.Add(chPlayer as Player);
        _behaviourFollowPlayer.players.Add(chPlayer as Player);
        
        GameObject goYodan1 = _gameManager.CreateCharacter("Yodan", enemy1Prefab, enemy1SpawnPoint.position, enemy1SpawnPoint.rotation * enemy1Prefab.transform.rotation, SideType.Enemy);
        Character chYodan1 = goYodan1.GetComponent<Character>();
        
        Enemy enemy1 = chYodan1 as Enemy;
        _attackController.SelectWeapon(enemy1, defaultEnemyWeapon);
        
        yield return null;
        
        while (chPlayer.IsMoving)
            yield return null;

        _uiManager.OpenWindow(ShopUI.gameObject, player as ICustomer);
        
        characterStateController.StateEnd(chPlayer, followPath);
        
        levelCamera.transform.SetParent(goPlayer.transform, true);
        
        yield return new WaitForSeconds(0.50f);
        
        _dialogManager.StartDialogue(DialogueEntrance);
        
        while (_dialogManager.IsDialogueRunning)
            yield return null;
        
         chYodan1.CurrentPath = enemy1Path;
         characterStateController.StateStart(chYodan1, followPath);
        
         yield return null;
         
         while (chYodan1.IsMoving)
             yield return null;
        
         yield return new WaitForSeconds(0.50f);
         
         _dialogManager.StartDialogue(DialogueYodan1);
        
         while (_dialogManager.IsDialogueRunning)
             yield return null;
        
         characterStateController.StateStart(chPlayer, playerInput);
         
         chYodan1.CurrentPath = new RandomPath(chYodan1);
         characterStateController.StateStart(chYodan1, followPlayer);
         characterStateController.StateStart(chYodan1, attackPlayer);
        
         GameObject goYodan2 = _gameManager.CreateCharacter("Yodan", enemy2Prefab, enemy2SpawnPoint.position, enemy2SpawnPoint.rotation * enemy2Prefab.transform.rotation, SideType.Enemy);
         Character chYodan2 = goYodan2.GetComponent<Character>();
         chYodan2.CurrentPath = enemy2Path;
         characterStateController.StateStart(chYodan2, followPath);
         characterStateController.StateStart(chYodan2, followPlayer);
         characterStateController.StateStart(chYodan2, attackPlayer);
         
         Enemy enemy2 = chYodan2 as Enemy;
         _attackController.SelectWeapon(enemy2, defaultEnemyWeapon);
         
         while (!chYodan1.IsDead || !chYodan2.IsDead)
            yield return null;
        
        yield return new WaitForSeconds(2);
        
        GameObject goVillager = _gameManager.CreateCharacter("Villager", villagerPrefab, villagerSpawnPoint.position, villagerSpawnPoint.rotation * villagerPrefab.transform.rotation, SideType.Friendly);
        Character chVillager = goVillager.GetComponent<Character>();
        chVillager.CurrentPath = villagerPath;
        characterStateController.StateStart(chVillager, followPath);
        
        characterStateController.StateEnd(chPlayer, playerInput);
        chPlayer.CurrentPath = playerPath2;
        characterStateController.StateStart(chPlayer, followPath);
        
        GameObject goSeller = _gameManager.CreateCharacter("Seller", sellerPrefab, sellerSpawnPoint.position, sellerSpawnPoint.rotation * sellerPrefab.transform.rotation, SideType.Friendly);
        Character chSeller = goSeller.GetComponent<Character>();
        chSeller.CurrentPath = sellerPath;
        characterStateController.StateStart(chSeller, followPath);
        
        yield return null;
        while (chVillager.IsMoving || chPlayer.IsMoving)
            yield return null;
        
        yield return new WaitForSeconds(1.0f);
         
        _dialogManager.StartDialogue(DialogueVillager);
        
        while (_dialogManager.IsDialogueRunning)
            yield return null;
        
        _spellController.AddSpell(player, fireBallSpell);
         Vector3 screenPos = levelCamera.WorldToScreenPoint(player.Position);
         _uiManager.CreateView(TextView.gameObject, "+ spell", screenPos.x, screenPos.y);
        
        characterStateController.StateEnd(chPlayer, followPath);
        characterStateController.StateStart(chPlayer, playerInput);
        characterStateController.StateStart(chPlayer, playerInteract);
    }
}
