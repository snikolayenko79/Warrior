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
        public string cameVar = "";
        public string villagerName = "";
        public float delay = 4.0f;

        private DiContainer _container;
        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private CharacterController _characterController;
        private BehaviourBase _behaviourFollowPath;
        
        [Inject]
        public void Construct(
            DiContainer container,
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            CharacterController characterController,
            BehaviourFollowPath behaviourFollowPath)
        {
            _container = container;
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _characterController = characterController;
            _behaviourFollowPath = behaviourFollowPath;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            yield return new WaitForSeconds(delay);
            
            GameObject go = _gameManager.CreateCharacter(villagerName, prefab, spawnPoint.position, spawnPoint.rotation * prefab.transform.rotation, SideType.Friendly);
        
            //Принудительно синхронизируем физику с текущим положением трансформы.
            // Если у этого персонажа есть Rigidbody, его буфер интерполяции мгновенно
            // примет эту точку за стартовую, и рывка вперед не произойдет.
            Physics.SyncTransforms();
            
            Character character = go.GetComponent<Character>();
            
            Door door = _container.ResolveId<Door>(doorId);
            door.Open(null);
            
            while (door && !door.IsOpened)
                yield return null;
            
            character.CurrentPath = path;
            _characterController.StateStart(character, _behaviourFollowPath);
        
            yield return null;
         
            while (character.IsMoving)
                yield return null;
            
            _scenarioManager.SetBool(cameVar, true);
        }
    }
}