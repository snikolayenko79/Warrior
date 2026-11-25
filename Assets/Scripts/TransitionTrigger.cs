using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Scenario;

public class TransitionTrigger : MonoBehaviour
{
    private ScenarioManager _scenarioManager;
    
    [Inject]
    public void Construct(ScenarioManager scenarioManager)
    {
        _scenarioManager = scenarioManager;
    }

    // Вызывается, например, при входе игрока в триггер-коллайдер двери перехода
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("q1");
        if (_scenarioManager.IsUnityNull())
            return;
        Debug.Log("q2");
        // Проверяем, что это именно игрок
        if (other.CompareTag("Player"))
            _scenarioManager.SetTrigger("ExitEnter", true);
    }
}