using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Scenario
{
    public class FightLogic : ScenarioStageLogic
    {
        public string fightEndedVar = "";
        
        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        
        List<Character> enemies = new List<Character>();
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            InitEnemiesList();

            float checkTimer = 0f;
            
            while (enemies.Count > 0)
            {
                checkTimer += Time.deltaTime;
        
                // Проверка на null срабатывает не каждый кадр!
                if (checkTimer >= 1.0f)
                {
                    checkTimer = 0f;
                    for (int i = enemies.Count - 1; i >= 0; i--)
                    {
                        if (enemies[i]?.IsUnityNull() ?? true)
                            enemies.RemoveAt(i);
                    }
                }
                
                yield return null;
            }

            _scenarioManager.SetBool(fightEndedVar, true);
        }

        private void InitEnemiesList()
        {
            enemies.Clear();
            
            foreach (var enemy in _gameManager.GetCharactersBySide(SideType.Enemy))
            {
                if (enemy.TryGetComponent<Character>(out var ch))
                {
                    enemies.Add(ch);
                    ch.OnDead += OnDead;
                }
            }
        }

        private void OnDead(Character ch)
        {
            enemies.Remove(ch);
            ch.OnDead -= OnDead;
        }
    }
}