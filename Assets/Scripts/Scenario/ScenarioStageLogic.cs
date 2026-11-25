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
            // Ожидание следующего кадра. Чтобы завершение логики гарантировано было на след. кадре после старта.
            // Это гарантирует правильный порядок обработки начала и завершения сценариев
            // Например, сброс и установку триггеров в менеджере (особенно актуально при запуске нескольких сценариев одновременно).
            yield return null;
            
            // Ждем выполнения логики наследника
            yield return StartCoroutine(ExecuteCoroutine());
        
            // Логика завершилась — вызываем колбэк
            onExecuteFinished?.Invoke();
        }

        // Наследники переопределяют этот метод
        protected abstract IEnumerator ExecuteCoroutine();
    }
}