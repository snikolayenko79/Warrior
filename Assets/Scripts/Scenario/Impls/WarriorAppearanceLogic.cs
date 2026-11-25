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

        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private IUIManager _uiManager;
        private IDialogManager _dialogManager;
        private IInventory _inventory;
        private IAttackController _attackController;
        private GameCharacterController _gameCharacterController;
        private BehaviourAttackPlayer _behaviourAttackPlayer;
        private BehaviourFollowPlayer _behaviourFollowPlayer;
        private BehaviourBase _behaviourFollowPath;
        private BehaviourBase _behaviourPlayerInput;
        private BehaviourBase _behaviourPlayerInteract;
        private CameraMoverBase _cameraMover;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            IUIManager uiManager,
            IDialogManager dialogManager,
            IAttackController attackController,
            IInventory inventory,
            GameCharacterController gameCharacterController,
            BehaviourAttackPlayer behaviourAttackPlayer,
            BehaviourFollowPlayer behaviourFollowPlayer,
            BehaviourFollowPath behaviourFollowPath,
            BehaviourPlayerInput behaviourPlayerInput,
            BehaviourPlayerInteract behaviourPlayerInteract,
            CameraMoverBase cameraMover)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _uiManager = uiManager;
            _dialogManager = dialogManager;
            _inventory = inventory;
            _attackController = attackController;
            _gameCharacterController = gameCharacterController;
            _behaviourAttackPlayer = behaviourAttackPlayer;
            _behaviourFollowPlayer = behaviourFollowPlayer;
            _behaviourFollowPath = behaviourFollowPath;
            _behaviourPlayerInput = behaviourPlayerInput;
            _behaviourPlayerInteract = behaviourPlayerInteract;
            _cameraMover = cameraMover;
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            _gameManager.SetGameplayLocked(true);
            
            bool isFirstAppear = Profile.GetVar<bool>("Warrior_firstAppear", true);
            if (isFirstAppear)
            {
                // На всякий случай очистим профиль игрока. Чтобы все в нуля начиналось.
                Profile.ResetPlayerAsync();
                _inventory.RemoveItemFromInventory(defaultInventoryItem);
            }

            // 1. СОЗДАЕМ ИГРОКА
            Player player = CreatePlayer(isFirstAppear);
        
            // 2. ДАЕМ ДЕФОЛТНОЕ ОРУЖИЕ ПРИ ПЕРВОМ ВХОДЕ
            if (isFirstAppear)
            {
                _inventory.AddItemToInventory(defaultInventoryItem);
                _attackController.SelectWeapon(player, defaultWeapon);
                
                // 3. ИДЕМ ОТ ТОЧКИ СПАВНА ДО ЗАДАННОЙ ПОЗИЦИИ
                player.CurrentPath = path;
                _gameCharacterController.StateStart(player, _behaviourFollowPath);
                yield return null;
                while (player.IsMoving)
                    yield return null;
                _gameCharacterController.StateEnd(player, _behaviourFollowPath);
                
                // 4. ПЕРВАЯ РЕПЛИКА ИГРОКА ПРИ ПЕРВОМ ВХОДЕ
                yield return new WaitForSeconds(0.50f);
                _dialogManager.StartDialogue(dialogue);
                while (_dialogManager.IsDialogueRunning)
                    yield return null;
            }

            // 5. ПРИВЯЗКА КАМЕРЫ К ИГРОКУ
            _cameraMover.SetTarget(player);
            
            // 6. ВКЛЮЧАЕМ ИНПУТ ДЛЯ ИГРОКА
            _gameCharacterController.StateStart(player, _behaviourPlayerInput);
            _gameCharacterController.StateStart(player, _behaviourPlayerInteract);

            Profile.SetVar<bool>("Warrior_firstAppear", false);
            _gameManager.SetGameplayLocked(false);
        }

        private Player CreatePlayer(bool isFirstAppear)
        {
            Transform pSpawn = isFirstAppear ? spawnPoint : path.GetFinishWayPoint();
            
            GameObject goPlayer = _gameManager.CreateCharacter("Warrior", prefab, pSpawn.position, pSpawn.rotation * prefab.transform.rotation, SideType.Player);
            Player player = goPlayer.GetComponent<Player>();
            
            // TODO. Как минимум отказаться от добавления прямо в список.
            _behaviourAttackPlayer.players.Add(player);
            _behaviourFollowPlayer.players.Add(player);
            
            // Принудительно синхронизируем физику с текущим положением трансформы.
            // Если у этого персонажа есть Rigidbody, его буфер интерполяции мгновенно
            // примет эту точку за стартовую, и рывка вперед не произойдет.
            Physics.SyncTransforms();

            return player;
        }
    }
}