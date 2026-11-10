using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Object = System.Object;

namespace Scenario
{
    public class ScenarioManager : IInitializable
    {
        private ScenarioData _scenarioData;

        private Dictionary<string, bool> _varsState;
        
        private List<ScenarioStage> _scenarioStagesToExecute = new List<ScenarioStage>();

        private DiContainer _diContainer = null;
        private CoroutineRunner _coroutineRunner = null;
        private Coroutine _coroutineCheckStages = null;
        
        public ScenarioManager(DiContainer diContainer, ScenarioData scenarioData, CoroutineRunner coroutineRunner)
        {
            _diContainer = diContainer;
            _coroutineRunner = coroutineRunner;
            _scenarioData = scenarioData;
        }

        public void Initialize()
        {
            InitVars();
            InitStages();
            CheckStages();
        }
        
        private void InitVars()
        {
            _varsState = new Dictionary<string, bool>();
            foreach (var scenarioVar in _scenarioData.ScenarioVars)
            {
                _varsState.Add(scenarioVar, false);
            }
        }

        private void InitStages()
        {
            foreach (var stage in _scenarioData.ScenarioStages)
            {
                _scenarioStagesToExecute.Add(stage);
            }
        }

        public bool GetBool(string varName)
        {
            _varsState.TryGetValue(varName, out bool varState);
            return varState;
        }
        
        public void SetBool(string varName, bool varState)
        {
            if (varName != null && _varsState.ContainsKey(varName))
            {
                _varsState[varName] = varState;
                
                if (_coroutineCheckStages == null)
                    _coroutineCheckStages = _coroutineRunner.StartCoroutine(CheckStagesOnNextFrame());
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
            for (int i = _scenarioStagesToExecute.Count - 1; i >= 0; i--)
            {
                
                ScenarioStage scenarioStage = _scenarioStagesToExecute[i];
                if (CheckDependencies(scenarioStage))
                {
                    StartStage(scenarioStage);
                    _scenarioStagesToExecute.RemoveAt(i);
                }
            }
        }
        
        private bool CheckDependencies(ScenarioStage scenarioStage)
        {
            foreach (var dependency in scenarioStage.Dependencies)
            {
                //Debug.Log(scenarioStage + " - " + dependency.varName + " - " + GetBool(dependency.varName) + " - " + dependency.varState);
                if (GetBool(dependency.varName) != dependency.varState)
                    return false;
            }
            
            return true;
        }

        private void StartStage(ScenarioStage scenarioStage)
        {
            if (!scenarioStage.LogicPrefab)
                return;
            
            ScenarioStageLogic logic = _diContainer.InstantiatePrefabForComponent<ScenarioStageLogic>(scenarioStage.LogicPrefab);
            
            logic.Execute(() =>
            {
                if (logic)
                    UnityEngine.Object.Destroy(logic.gameObject);
            });
        }
    }
}
