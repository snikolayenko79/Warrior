using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Scenario
{
    public class ExitLogic : ScenarioStageLogic
    {
        [SerializeField] private Dialogue cantLeaveVillagers = null;
        [SerializeField] private Dialogue cantLeaveMonsters = null;
        [SerializeField] private string moveToSceneName = "";
        [SerializeField] private SpecifiedPath path;
        
        private ScenarioManager _scenarioManager;
        private IDialogManager _dialogueManager;
        private ISceneLoader _sceneLoader;
        private Collider _blocker;
        private BehaviourBase _behaviourFollowPath;
        private GameCharacterController _gameCharacterController;
        private IGameManager _gameManager;
        private CameraMoverBase _cameraMover;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IDialogManager dialogueManager, 
            ISceneLoader sceneLoader,
            Collider blocker,
            BehaviourFollowPath behaviourFollowPath,
            GameCharacterController gameCharacterController,
            IGameManager gameManager,
            CameraMoverBase cameraMover)
        {
            _scenarioManager = scenarioManager;
            _dialogueManager = dialogueManager;
            _sceneLoader = sceneLoader;
            _blocker = blocker;
            _behaviourFollowPath = behaviourFollowPath;
            _gameCharacterController = gameCharacterController;
            _gameManager = gameManager;
            _cameraMover = cameraMover;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            // ЕЩЕ НЕ УБИЛИ ВСЕХ МОНСТРОВ
            if (!Profile.GetVar<bool>("AllEnemiesDie", false))
            {
                _dialogueManager.StartDialogue(cantLeaveMonsters);
                yield break;
            }
            
            // ЕЩЕ НЕ ПОГОВОРИЛИ С ЖИТЕЛЯМИ
            if (!Profile.GetVar<bool>("VillagerInteract", false) || !Profile.GetVar<bool>("SellerInteract", false))
            {
                _dialogueManager.StartDialogue(cantLeaveVillagers);
                yield break;
            }
            
            // ОТВЯЗКА КАМЕРЫ ОТ ИГРОКА
            _cameraMover.SetTarget(null);
            
            // ПЕРЕХОД НА СЛЕД. СЦЕНУ
            _gameManager.SetGameplayLocked(true);
            if (_blocker)
                _blocker.gameObject.SetActive(false);
            if (path)
            {
                ICharacter character = _gameManager.PlayerCharacter;
                if (character is Player player)
                {
                    player.CurrentPath = path;
                    _gameCharacterController.StateStart(player, _behaviourFollowPath);
                    yield return null;
                    while (player.IsMoving)
                        yield return null;
                }
            }
            _gameManager.SetGameplayLocked(false);
            
            // ЗАГРУЗКА НОВОЙ СЦЕНЫ
            _sceneLoader.LoadScene(moveToSceneName); 
        }
    }
}