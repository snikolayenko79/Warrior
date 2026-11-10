using UnityEngine;
using System.Collections;
using UnityEditorInternal;
using Zenject;

namespace Scenario
{
    public class VillagerAppearanceLogic : ScenarioStageLogic
    {
        public GameObject prefab;
        public Transform spawnPoint;
        public SpecifiedPath path;
        public string cameVar = "";
        public string villagerName = "";
        public float delay = 4.0f;

        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private CharacterStateController _characterStateController;
        private BehaviourBase _behaviourFollowPath;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            CharacterStateController characterStateController,
            BehaviourFollowPath behaviourFollowPath)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _characterStateController = characterStateController;
            _behaviourFollowPath = behaviourFollowPath;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            _gameManager.SetGameplayLocked(true);
            
            yield return new WaitForSeconds(delay);
            
            GameObject go = _gameManager.CreateCharacter(villagerName, prefab, spawnPoint.position, spawnPoint.rotation * prefab.transform.rotation, SideType.Enemy);
            Character character = go.GetComponent<Character>();
        
            yield return null;
            
            character.CurrentPath = path;
            _characterStateController.StateStart(character, _behaviourFollowPath);
        
            yield return null;
         
            while (character.IsMoving)
                yield return null;
            
            _scenarioManager.SetBool(cameVar, true);
            
            _gameManager.SetGameplayLocked(false);
        }
    }
}