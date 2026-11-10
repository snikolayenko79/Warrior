using UnityEngine;
using System.Collections.Generic;

namespace Scenario
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "Scenario/New scenario data")]
    public class ScenarioData : ScriptableObject
    {
        [SerializeField] private List<ScenarioStage> scenarioStages = new List<ScenarioStage>();
        [SerializeField] private List<string> scenarioVars = new List<string>();
        
        public IReadOnlyCollection<ScenarioStage> ScenarioStages => scenarioStages;
        public IReadOnlyCollection<string> ScenarioVars => scenarioVars;
    }
}