using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Object = System.Object;

namespace Scenario
{
    public class ScenarioManager : IInitializable
    {
        private readonly ScenarioData _scenarioData;

        private Dictionary<string, bool> _triggersState;
        
        private readonly List<ScenarioStage> _scenarioStagesToExecute = new List<ScenarioStage>();
        private List<ScenarioStage> _scenarioStagesCanExecute = new List<ScenarioStage>();
        
        private readonly DiContainer _diContainer = null;
        private readonly CoroutineRunner _coroutineRunner = null;
        private Coroutine _coroutineCheckStages = null;
        
        public ScenarioManager(DiContainer diContainer, ScenarioData scenarioData, CoroutineRunner coroutineRunner)
        {
            _diContainer = diContainer;
            _coroutineRunner = coroutineRunner;
            _scenarioData = scenarioData;
        }

        public void Initialize()
        {
            InitTriggers();
            InitStages();
            CheckStages();
        }
        
        private void InitTriggers()
        {
            _triggersState = new Dictionary<string, bool>();
            foreach (var trigger in _scenarioData.ScenarioTriggers)
            {
                _triggersState.Add(trigger, false);
            }
            foreach (var trigger in _scenarioData.ScenarioAutoTriggers)
            {
                _triggersState.Add(trigger, true);
            }
        }

        private void InitStages()
        {
            foreach (var stage in _scenarioData.ScenarioStages)
            {
                _scenarioStagesToExecute.Add(stage);
            }
        }

        public bool GetTrigger(string name)
        {
            _triggersState.TryGetValue(name, out bool varState);
            return varState;
        }
        
        public void SetTrigger(string name, bool state)
        {
            if (name != null && _triggersState.ContainsKey(name))
            {
                _triggersState[name] = state;
                
                //Debug.Log("Set trigger " + name + ": " + state);

                if (state == true)
                {
                    if (_coroutineCheckStages == null)
                        _coroutineCheckStages = _coroutineRunner.StartCoroutine(CheckStagesOnNextFrame());
                }
            }
        }

        private IEnumerator CheckStagesOnNextFrame()
        {
            yield return  null;
            _coroutineCheckStages = null; //TODO? обязательно до CheckStages. чтобы уже запускалась новая корутина.
            CheckStages();
        }
        
        private void CheckStages()
        {
            _scenarioStagesCanExecute.Clear();
            for (int i = _scenarioStagesToExecute.Count - 1; i >= 0; i--)
            {
                ScenarioStage scenarioStage = _scenarioStagesToExecute[i];
                if (GetTrigger(scenarioStage.StartTrigger))
                    _scenarioStagesCanExecute.Add(scenarioStage);
            }

            for (int i = _scenarioStagesCanExecute.Count - 1; i >= 0; i--)
            {
                ScenarioStage scenarioStage = _scenarioStagesCanExecute[i];
                SetTrigger(scenarioStage.StartTrigger, false);
                StartStage(scenarioStage);
            }
        }

        private void StartStage(ScenarioStage scenarioStage)
        {
            if (!scenarioStage.LogicPrefab)
                return;
            
            ScenarioStageLogic logic = _diContainer.InstantiatePrefabForComponent<ScenarioStageLogic>(scenarioStage.LogicPrefab);
            
            logic.Execute(() =>
            {
                if (scenarioStage && scenarioStage.EndTrigger != string.Empty)
                    SetTrigger(scenarioStage.EndTrigger, true);
                
                if (logic)
                    UnityEngine.Object.Destroy(logic.gameObject);
            });
        }
    }
}
