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
        public AudioSource roarSound;
        
        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private IDialogManager _dialogManager;
        private IAttackController _attackController;
        private GameCharacterController _gameCharacterController;
        private BehaviourBase _behaviourAttackPlayer;
        private BehaviourBase _behaviourFollowPlayer;
        private BehaviourBase _behaviourFollowPath;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            IDialogManager dialogManager,
            IAttackController attackController,
            GameCharacterController gameCharacterController,
            BehaviourAttackPlayer behaviourAttackPlayer,
            BehaviourFollowPlayer behaviourFollowPlayer,
            BehaviourFollowPath behaviourFollowPath)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _dialogManager = dialogManager;
            _attackController = attackController;
            _gameCharacterController = gameCharacterController;
            _behaviourAttackPlayer = behaviourAttackPlayer;
            _behaviourFollowPlayer = behaviourFollowPlayer;
            _behaviourFollowPath = behaviourFollowPath;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            // Если уже был убит, то не спавним.
            if (Profile.GetVar<bool>("Yodan1_dead", false))
            {
                yield break;
            }

            bool isFirstAppear = Profile.GetVar<bool>("Yodan1_firstAppear", true);
            
            // СОЗДАЕМ ВРАГА
            Enemy enemy = CreateYodan();
            
            // ДАЕМ ДЕФОЛТНОЕ ОРУЖИЕ. У ВРАГОВ ОНО НЕ СОХРАНЯЕТСЯ
            _attackController.SelectWeapon(enemy, defaultWeapon);
            
            if (isFirstAppear)
            {
                _gameManager.SetGameplayLocked(true);
                
                // ЗВУК ПРИ ПЕРВОМ ВЫХОДЕ
                if (roarSound)
                    roarSound.Play();
                yield return new WaitForSeconds(0.5f);
                
                // ИДЕМ ОТ ТОЧКИ СПАВНА ДО ЗАДАННОЙ ПОЗИЦИИ
                enemy.CurrentPath = path;
                _gameCharacterController.StateStart(enemy, _behaviourFollowPath);
                yield return null;
                while (enemy.IsMoving)
                    yield return null;
                
                // ДИАЛОГ С ИГРОКОМ ПРИ 1-М ВЫХОДЕ
                yield return new WaitForSeconds(0.50f);
                if (!dialogue.IsUnityNull())
                    _dialogManager.StartDialogue(dialogue);
                while (_dialogManager.IsDialogueRunning)
                    yield return null;
                
                _gameManager.SetGameplayLocked(false);
            }
            else
            {
                // ХОДИМ ПО РАНДОМНОМУ МАРШРУТУ
                enemy.CurrentPath = new RandomPath(enemy);
                _gameCharacterController.StateStart(enemy, _behaviourFollowPath);
            }

            // ДАЕМ ВОЗМОЖНОСТИ ПРЕСЛЕДОВАТЬ ИГРОКА И АТАКОВАТЬ
            _gameCharacterController.StateStart(enemy, _behaviourAttackPlayer);
            _gameCharacterController.StateStart(enemy, _behaviourFollowPlayer);
            
            Profile.SetVar<bool>("Yodan1_firstAppear", false);
        }
        
        private Enemy CreateYodan()
        {
            GameObject go = _gameManager.CreateCharacter("Yodan1", prefab, spawnPoint.position, spawnPoint.rotation * prefab.transform.rotation, SideType.Enemy);
            Enemy enemy = go.GetComponent<Enemy>();
            
            // Принудительно синхронизируем физику с текущим положением трансформы.
            // Если у этого персонажа есть Rigidbody, его буфер интерполяции мгновенно
            // примет эту точку за стартовую, и рывка вперед не произойдет.
            Physics.SyncTransforms();

            return enemy;
        }
    }
}