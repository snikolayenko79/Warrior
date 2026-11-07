using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Scenario
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private List<ScenarioStage> scenarioStages = new List<ScenarioStage>();
        [SerializeField] private List<string> scenarioVars = new List<string>();

        private Dictionary<string, bool> _varsState;
        
        private CoroutineRunner _coroutineRunner = null;
        private Coroutine _coroutineCheckStages = null;
        
        public void Construct(CoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        private IEnumerator Start()
        {
            InitVars();
            
            yield return null;
            CheckStages();
        }
        
        private void InitVars()
        {
            _varsState = new Dictionary<string, bool>();
            foreach (var scenarioVar in scenarioVars)
            {
                _varsState.Add(scenarioVar, false);
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
            
            CheckStages();
            _coroutineCheckStages = null;
        }
        
        private void CheckStages()
        {
            foreach (var scenarioStage in scenarioStages)
            {
                if (scenarioStage.IsStarted ||  scenarioStage.IsEnded)
                    continue;
                
                if (scenarioStage.CheckDependencies())
                    scenarioStage.StartStage();
            }
        }
    }
}
