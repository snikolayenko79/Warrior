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
        [SerializeField] private string startTrigger;
        [SerializeField] private string endTrigger = string.Empty;
        [SerializeField] private ScenarioStageLogic scenarioLogicPrefab;
        
        public string StartTrigger => startTrigger;
        public string EndTrigger => endTrigger;
        public ScenarioStageLogic LogicPrefab => scenarioLogicPrefab;
    }
}