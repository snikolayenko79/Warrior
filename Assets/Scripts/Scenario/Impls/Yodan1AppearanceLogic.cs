using UnityEngine;
using System.Collections;
using UnityEditorInternal;
using Zenject;

namespace Scenario
{
    public class Yodan1AppearanceLogic : ScenarioStageLogic
    {
        public GameObject prefab;
        public WeaponData defaultWeapon;
        public Transform spawnPoint;
        public SpecifiedPath path;
        public Dialogue dialogue = null;
        public string cameVar = "";

        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private IDialogManager _dialogManager;
        private IAttackController _attackController;
        private CharacterStateController _characterStateController;
        private BehaviourBase _behaviourAttackPlayer;
        private BehaviourBase _behaviourFollowPlayer;
        private BehaviourBase _behaviourFollowPath;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            IDialogManager dialogManager,
            IAttackController attackController,
            CharacterStateController characterStateController,
            BehaviourAttackPlayer behaviourAttackPlayer,
            BehaviourFollowPlayer behaviourFollowPlayer,
            BehaviourFollowPath behaviourFollowPath)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _dialogManager = dialogManager;
            _attackController = attackController;
            _characterStateController = characterStateController;
            _behaviourAttackPlayer = behaviourAttackPlayer;
            _behaviourFollowPlayer = behaviourFollowPlayer;
            _behaviourFollowPath = behaviourFollowPath;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            _gameManager.SetGameplayLocked(true);
            
            GameObject go = _gameManager.CreateCharacter("Yodan1", prefab, spawnPoint.position, spawnPoint.rotation * prefab.transform.rotation, SideType.Enemy);
            Character character = go.GetComponent<Character>();
        
            Enemy enemy = character as Enemy;
            _attackController.SelectWeapon(enemy, defaultWeapon);
        
            yield return null;
            
            character.CurrentPath = path;
            _characterStateController.StateStart(character, _behaviourFollowPath);
        
            yield return null;
         
            while (character.IsMoving)
                yield return null;
        
            yield return new WaitForSeconds(0.50f);
         
            if (!dialogue.IsUnityNull())
                _dialogManager.StartDialogue(dialogue);
        
            while (_dialogManager.IsDialogueRunning)
                yield return null;
            
            _characterStateController.StateStart(character, _behaviourAttackPlayer);
            _characterStateController.StateStart(character, _behaviourFollowPlayer);
            
            _scenarioManager.SetBool(cameVar, true);
            
            _gameManager.SetGameplayLocked(false);
        }
    }
}