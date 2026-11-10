using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Zenject;

namespace Scenario
{
    [CreateAssetMenu(fileName = "NewScenarioStage", menuName = "Scenario/New scenario stage data")]
    public class ScenarioStage : ScriptableObject
    {
        [System.Serializable]
        public class Dependency
        {
            public string varName;
            public bool varState;
        }

        [SerializeField] private List<Dependency> dependencies;
        [SerializeField] private ScenarioStageLogic scenarioLogicPrefab;
        
        public IReadOnlyCollection<Dependency> Dependencies => dependencies;
        public ScenarioStageLogic LogicPrefab => scenarioLogicPrefab;
    }
}