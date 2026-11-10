using System;
using UnityEngine;
using System.Collections;
using UnityEditorInternal;
using Zenject;

namespace  Scenario
{
    public class InteractLogic : ScenarioStageLogic
    {
        public Dialogue dialogueVillager1;
        public Dialogue dialogueVillager2;
        public Dialogue dialogueSeller;
        public SpellData fireBallSpell;
        public UIView TextViewPrefab;
        public ShopWindowUI ShopUIPrefab;
        
        private ScenarioManager _scenarioManager;
        private IGameManager _gameManager;
        private IDialogManager _dialogManager;
        private IUIManager _uiManager;
        private SpellController _spellController;
        private BehaviourPlayerInteract _behaviourInteract;

        private Dialogue _currentDialogue;
        private bool _dialogVillager1Ended = false;
        private bool _dialogSellerEnded = false;
        
        private delegate void OnDialogueEnd();
        
        [Inject]
        public void Construct(
            ScenarioManager scenarioManager,
            IGameManager gameManager,
            IDialogManager dialogManager,
            IUIManager uiManager,
            SpellController spellController,
            BehaviourPlayerInteract behaviourInteract)
        {
            _scenarioManager = scenarioManager;
            _gameManager = gameManager;
            _dialogManager = dialogManager;
            _uiManager = uiManager;
            _spellController = spellController;
            _behaviourInteract = behaviourInteract;
        }
        
        protected override IEnumerator ExecuteCoroutine()
        {
            _behaviourInteract.OnInteract += OnInteract;
            
            while (true)
                yield return null;
            
            _behaviourInteract.OnInteract -= OnInteract;
        }

        private void OnInteract(IInteractor playerInteractor, IInteractor interactor)
        {
            if (interactor is IMovable movable)
            {
                if (movable.IsMoving)
                    return;
            }
            
            if (interactor.Name == "Villager")
            {
                if (_dialogVillager1Ended)
                    StartCoroutine(ShowDialogue(dialogueVillager2, null));
                else
                    StartCoroutine(ShowDialogue(dialogueVillager1, () => AddSpell(playerInteractor as ISpellCaster)));
            }
            else if (interactor.Name == "Seller")
            {
                if (_dialogSellerEnded)
                    ShowStore(playerInteractor as ICustomer);
                else
                    StartCoroutine(ShowDialogue(dialogueSeller, () =>
                        {
                            Heal(playerInteractor as IDamageable);
                            ShowStore(playerInteractor as ICustomer);
                        }
                    ));
            }
        }

        private IEnumerator ShowDialogue(Dialogue dialogue, OnDialogueEnd onEnded)
        {
            _gameManager.SetGameplayLocked(true);
            
            _dialogManager.StartDialogue(dialogue);
            
            while (_dialogManager.IsDialogueRunning)
                yield return null;

            yield return new WaitForSeconds(0.3f);
            
            _gameManager.SetGameplayLocked(false);
            
            onEnded?.Invoke();
        }

        private void AddSpell(ISpellCaster spellCaster)
        {
            if (spellCaster.IsUnityNull())
                return;
            
            _spellController.AddSpell(spellCaster, fireBallSpell);

            if (spellCaster is IMovable movable)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(movable.Position);
                _uiManager.CreateView(TextViewPrefab.gameObject, "+ spell", screenPos.x, screenPos.y);
            }

            _dialogVillager1Ended = true;
        }

        private void Heal(IDamageable damageable)
        {
            if (damageable.IsUnityNull())
                return;
            
            float healthBefore = damageable.Health;
            damageable.Health += 50;

            float heal = (damageable.Health - healthBefore);
            if (heal > 0)
            {
                if (damageable is IMovable movable)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(movable.Position);
                    _uiManager.CreateView(TextViewPrefab.gameObject,
                        "+" + (damageable.Health - healthBefore).ToString() + " health points", screenPos.x,
                        screenPos.y);
                }
            }
        }
        
        private void ShowStore(ICustomer customer)
        {
            _uiManager.OpenWindow(ShopUIPrefab.gameObject, customer);
            _dialogSellerEnded = true;
        }
    }
}