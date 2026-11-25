using UnityEngine;
using System.Collections;
using UnityEditorInternal;
using Zenject;

namespace Scenario
{
    public class VillagerAppearanceLogic : ScenarioStageLogic
    {
        public string doorId = "doorVillager";
        public GameObject prefab;
        public Transform spawnPoint;
        public SpecifiedPath path;
        public string villagerName = "";
        public float delay = 4.0f;

        private DiContainer _container;
        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private GameCharacterController _gameCharacterController;
        private BehaviourBase _behaviourFollowPath;
        
        [Inject]
        public void Construct(
            DiContainer container,
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            GameCharacterController gameCharacterController,
            BehaviourFollowPath behaviourFollowPath)
        {
            _container = container;
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _gameCharacterController = gameCharacterController;
            _behaviourFollowPath = behaviourFollowPath;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            bool isFirstAppear = Profile.GetVar<bool>(villagerName + "_firstAppear", true);
            //Profile.SetVar<bool>(villagerName + "_firstAppear", false);

            // СОЗДАНИЕ ЖИТЕЛЯ
            Character villager = CreateVillager(isFirstAppear);
            
            if (isFirstAppear)
                yield return new WaitForSeconds(delay);
            
            // ОТКРЫТИЕ ДВЕРЕЙ ДОМА
            Door door = _container.ResolveId<Door>(doorId);
            door.Open(!isFirstAppear, null);
            while (door && !door.IsOpened)
                yield return null;
            
            // ЖИТЕЛЬ ВЫХОДИТ ИЗ ДОМА
            if (isFirstAppear)
            {
                villager.CurrentPath = path;
                _gameCharacterController.StateStart(villager, _behaviourFollowPath);
                yield return null;
                while (villager.IsMoving)
                    yield return null;
                _gameCharacterController.StateEnd(villager, _behaviourFollowPath);
            }
        }
        
        private Character CreateVillager(bool isFirstAppear)
        {
            Transform pSpawn = isFirstAppear ? spawnPoint : path.GetFinishWayPoint();
            GameObject go = _gameManager.CreateCharacter(villagerName, prefab, pSpawn.position, pSpawn.rotation * prefab.transform.rotation, SideType.Friendly);
        
            //Принудительно синхронизируем физику с текущим положением трансформы.
            // Если у этого персонажа есть Rigidbody, его буфер интерполяции мгновенно
            // примет эту точку за стартовую, и рывка вперед не произойдет.
            Physics.SyncTransforms();
            
            return go.GetComponent<Character>();
        }
    }
}