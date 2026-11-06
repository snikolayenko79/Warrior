using UnityEngine;
using Zenject;
using System;

public class BehaviourPlayerInteract : BehaviourBase
{
    public float interactDistance = 0.3f;
    
    public Dialogue DialogueVillager = null;
    public Dialogue DialogueSeller = null;
    
    private IGameManager _gameManager;
    private IDialogManager _dialogManager;
    
    public Action<IInteractor, IInteractor> OnInteract;

    [Inject]
    public void Construct(IGameManager gameManager, IDialogManager dialogManager)
    {
        _gameManager = gameManager;
        _dialogManager = dialogManager;
    }
    
    private IInteractor _playerInteractor;
    private IMovable _playerMovable;
    private IInputListener _inputListener;
    
    public void SetInputListener(IInputListener inputListener)
    {
    }
    
    public override void StateStart(ICharapterState characterState)
    {
        _playerInteractor = characterState as IInteractor;
        _playerMovable = characterState as IMovable;
        
        TryGetComponent<IInputListener>(out _inputListener);

        if (_inputListener != null)
        {
            _inputListener.OnInputAction += OnInputAction;
        }
    }
    
    public override void StateEnd(ICharapterState characterState)
    {
        if (_inputListener != null)
        {
            _inputListener.OnInputAction -= OnInputAction;
        }
    }

    public override void StateUpdate(ICharapterState characterState)
    {
    }

    private void OnInputAction(string logicalActionName, InputContext context)
    {
        if (logicalActionName == "Attack" && context.FloatValue == 0)
        {
            if (_playerInteractor.IsUnityNull() || _playerInteractor.IsDead)
                return;
            
            foreach (var character in _gameManager.GetCharactersBySide(SideType.Friendly))
            {
                if (!character.TryGetComponent<IInteractor>(out IInteractor interactor))
                    continue;
                    
                if (CanInteractWith(interactor))
                {
                    OnInteract?.Invoke(_playerInteractor, interactor);
                    break;
                }
            }
        }
    }

    private bool CanInteractWith(IInteractor interactor)
    {
        if (interactor is IMovable movable && !_playerMovable.IsUnityNull())
            return Vector3.Distance(movable.Position, _playerMovable.Position) <= interactDistance;

        return true;
    }
}