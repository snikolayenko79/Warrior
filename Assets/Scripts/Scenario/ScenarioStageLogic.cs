using System.Collections;
using UnityEngine;

namespace Scenario
{
    public abstract class ScenarioStageLogic : MonoBehaviour
    {
        public delegate void OnExecuteFinished();
        
        public void Execute(OnExecuteFinished onExecuteFinished)
        {
            StartCoroutine(ExecuteCoroutine(onExecuteFinished));
        }

        protected abstract IEnumerator ExecuteCoroutine(OnExecuteFinished onExecuteFinished);
    }
}