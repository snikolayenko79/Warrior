using System.Collections;
using UnityEngine;

namespace Scenario
{
    public abstract class ScenarioStageLogic : MonoBehaviour
    {
        public delegate void OnExecuteFinished();
        
        public void Execute(OnExecuteFinished onExecuteFinished)
        {
            StartCoroutine(ExecuteWrapper(onExecuteFinished));
        }

        // Базовая обертка, которая гарантирует вызов делегата в конце
        private IEnumerator ExecuteWrapper(OnExecuteFinished onExecuteFinished)
        {
            // Ждем выполнения логики наследника
            yield return StartCoroutine(ExecuteCoroutine());
        
            // Логика завершилась — вызываем колбэк
            onExecuteFinished?.Invoke();
        }

        // Наследники переопределяют этот метод
        protected abstract IEnumerator ExecuteCoroutine();
    }
}