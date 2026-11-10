using UnityEngine;
using System.Collections;
using Zenject;

namespace Scenario
{
    public class WarriorAppearanceLogic : ScenarioStageLogic
    {
        public GameObject prefab;
        public InventoryItem defaultInventoryItem;
        public WeaponData defaultWeapon;
        public Transform spawnPoint;
        public SpecifiedPath path;
        public Dialogue dialogue = null;
        public string cameVar = "";

        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private IUIManager _uiManager;
        private IDialogManager _dialogManager;
        private IInventory _inventory;
        private IAttackController _attackController;
        private CharacterStateController _characterStateController;
        private BehaviourAttackPlayer _behaviourAttackPlayer;
        private BehaviourFollowPlayer _behaviourFollowPlayer;
        private BehaviourBase _behaviourFollowPath;
        private BehaviourBase _behaviourPlayerInput;
        private BehaviourBase _behaviourPlayerInteract;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            IUIManager uiManager,
            IDialogManager dialogManager,
            IAttackController attackController,
            IInventory inventory,
            CharacterStateController characterStateController,
            BehaviourAttackPlayer behaviourAttackPlayer,
            BehaviourFollowPlayer behaviourFollowPlayer,
            BehaviourFollowPath behaviourFollowPath,
            BehaviourPlayerInput behaviourPlayerInput,
            BehaviourPlayerInteract behaviourPlayerInteract)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _uiManager = uiManager;
            _dialogManager = dialogManager;
            _inventory = inventory;
            _attackController = attackController;
            _characterStateController = characterStateController;
            _behaviourAttackPlayer = behaviourAttackPlayer;
            _behaviourFollowPlayer = behaviourFollowPlayer;
            _behaviourFollowPath = behaviourFollowPath;
            _behaviourPlayerInput = behaviourPlayerInput;
            _behaviourPlayerInteract = behaviourPlayerInteract;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            _gameManager.SetGameplayLocked(true);
            
            yield return new WaitForSeconds(3);
        
            GameObject goPlayer = _gameManager.CreateCharacter("Warrior", prefab, spawnPoint.position, spawnPoint.rotation * prefab.transform.rotation, SideType.Player);
        
            Character chPlayer = goPlayer.GetComponent<Character>();
        
            _inventory.AddItemToInventory(defaultInventoryItem);

            chPlayer.CurrentPath = path;
            _characterStateController.StateStart(chPlayer, _behaviourFollowPath);
        
            Player player = chPlayer as Player;
            _attackController.SelectWeapon(player, defaultWeapon);
        
            // TODO. Как минимум отказаться от добавления прямо в список.
            _behaviourAttackPlayer.players.Add(chPlayer as Player);
            _behaviourFollowPlayer.players.Add(chPlayer as Player);
        
            yield return null;
        
            while (chPlayer.IsMoving)
                yield return null;
        
            _characterStateController.StateEnd(chPlayer, _behaviourFollowPath);
        
            yield return new WaitForSeconds(0.50f);
        
            _dialogManager.StartDialogue(dialogue);
        
            while (_dialogManager.IsDialogueRunning)
                yield return null;
            
            _characterStateController.StateStart(chPlayer, _behaviourPlayerInput);
            _characterStateController.StateStart(chPlayer, _behaviourPlayerInteract);
            
            _scenarioManager.SetBool(cameVar, true);
            
            _gameManager.SetGameplayLocked(false);
        }
    }
}