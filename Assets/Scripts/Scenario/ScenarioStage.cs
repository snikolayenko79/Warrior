using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace Scenario
{
    public class ScenarioStage : MonoBehaviour
    {
        [System.Serializable]
        public class Dependency
        {
            public string varName;
            public bool varState;
        }

        [SerializeField] private List<Dependency> dependencies;
        [SerializeField] private ScenarioLogic scenarioLogic;

        private bool _isStarted = false;
        public bool IsStarted => _isStarted;
        
        private bool _isEnded = false;
        public bool IsEnded => _isEnded;
        
        private ScenarioManager _manager;

        public void Construct(ScenarioManager manager)
        {
            _manager = manager;
        }
        
        private bool CheckDependency(Dependency dependency)
        {
            bool varState = _manager.GetBool(dependency.varName);
            return  (varState && dependency.varState);
        }
        
        public bool CheckDependencies()
        {
            foreach (var dependency in dependencies)
            {
                if (!CheckDependency(dependency))
                    return false;
            }
            
            return true;
        }
        
        public void StartStage()
        {
            if (scenarioLogic == null)
            {
                Debug.LogError($"[ScenarioStage] На объекте {name} отсутствует обязательный компонент логики ScenarioLogic!", this);
                return;
            }
            
            _isStarted = true;
            _isEnded = false;
            
            scenarioLogic.Execute(() =>
            {
                _isStarted = false;
                _isEnded = true;
            });
        }
    }
}