using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Scenario
{
    public class ExitLogic : ScenarioStageLogic
    {
        [SerializeField] private string moveToSceneName = "";
        [SerializeField] private Dialogue cantLeaveVillagers = null;
        [SerializeField] private Dialogue cantLeaveMonsters = null;
        
        private ScenarioManager _scenarioManager;
        private IDialogManager _dialogueManager;
        private ISceneLoader _sceneLoader;
        private Collider _blocker;
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IDialogManager dialogueManager, 
            ISceneLoader sceneLoader,
            Collider blocker)
        {
            _scenarioManager = scenarioManager;
            _dialogueManager = dialogueManager;
            _sceneLoader = sceneLoader;
            _blocker = blocker;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            if (!Profile.GetVar<bool>("AllEnemiesDie", false))
            {
                _dialogueManager.StartDialogue(cantLeaveMonsters);
                yield break;
            }
            
            if (!Profile.GetVar<bool>("VillagerInteract", false) || !Profile.GetVar<bool>("SellerInteract", false))
            {
                _dialogueManager.StartDialogue(cantLeaveVillagers);
                yield break;
            }
            
            if (_blocker)
                _blocker.gameObject.SetActive(false);

            // Загружаем следующий уровень
            _sceneLoader.LoadScene(moveToSceneName); 
        }
    }
}