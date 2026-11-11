using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Scenario
{
    public class YodanSpawnLogic : ScenarioStageLogic
    {
        public List<GameObject> prefabs;
        public WeaponData defaultWeapon;
        public List<Transform> spawnPoints;
        public int startNameIndex;
        public string spawnedVar = "";
        
        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private IAttackController _attackController;
        private CharacterStateController _characterStateController;
        private BehaviourBase _behaviourFollowPath;
        private BehaviourBase _behaviourAttackPlayer;
        private BehaviourBase _behaviourFollowPlayer;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            IDialogManager dialogManager,
            IAttackController attackController,
            CharacterStateController characterStateController,
            BehaviourFollowPath behaviourFollowPath,
            BehaviourAttackPlayer behaviourAttackPlayer,
            BehaviourFollowPlayer behaviourFollowPlayer)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _attackController = attackController;
            _characterStateController = characterStateController;
            _behaviourFollowPath = behaviourFollowPath;
            _behaviourAttackPlayer = behaviourAttackPlayer;
            _behaviourFollowPlayer = behaviourFollowPlayer;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                GameObject prefab = prefabs[i];
                Transform spawnPoint = spawnPoints[i];
                GameObject go = _gameManager.CreateCharacter("Yodan" + startNameIndex++.ToString(), prefab, spawnPoint.position, spawnPoint.rotation * prefab.transform.rotation, SideType.Enemy);
                Character character = go.GetComponent<Character>();
        
                Enemy enemy = character as Enemy;
                _attackController.SelectWeapon(enemy, defaultWeapon);

                if (character is IMovable movable)
                {
                    movable.CurrentPath = new RandomPath(movable);
                    _characterStateController.StateStart(character, _behaviourFollowPath);
                    _characterStateController.StateStart(character, _behaviourAttackPlayer);
                    _characterStateController.StateStart(character, _behaviourFollowPlayer);
                }
            }
            
            //Принудительно синхронизируем физику с текущим положением трансформы.
            // Если у этого персонажа есть Rigidbody, его буфер интерполяции мгновенно
            // примет эту точку за стартовую, и рывка вперед не произойдет.
            Physics.SyncTransforms();
            
            _scenarioManager.SetBool(spawnedVar, true);
            
            yield break;
        }
    }
}