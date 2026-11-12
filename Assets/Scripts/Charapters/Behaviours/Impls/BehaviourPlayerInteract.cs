using UnityEngine;
using Zenject;
using System;

public class BehaviourPlayerInteract : BehaviourBase
{
    public float interactDistance = 0.3f;
    
    public Action<IInteractor, IInteractor> OnInteract;
    
    private IInteractor _playerInteractor;
    private IMovable _playerMovable;
    private IInputListener _inputListener;
    
    public void SetInputListener(IInputListener inputListener)
    {
    }
    
    public override void StateStart(ICharacter character)
    {
        _playerInteractor = character as IInteractor;
        _playerMovable = character as IMovable;
        
        TryGetComponent<IInputListener>(out _inputListener);

        if (_inputListener != null)
        {
            _inputListener.OnInputAction += OnInputAction;
        }
    }
    
    public override void StateEnd(ICharacter character)
    {
        if (_inputListener != null)
        {
            _inputListener.OnInputAction -= OnInputAction;
        }
    }

    public override void StateUpdate(ICharacter character)
    {
    }

    private void OnInputAction(string logicalActionName, InputContext context)
    {
        if (_gameManager.IsUnityNull() || _gameManager.IsGameplayLocked())
            return;
        
        if (_playerInteractor.IsUnityNull() || _playerInteractor.IsDead)
            return;
        
        if (logicalActionName == "Attack" && context.FloatValue == 0)
        {
            var characters = _gameManager.GetAllCharacters();
            for (int i = 0; i < characters.Count; i++)
            {
                var character = characters[i];
                
                if (character.IsUnityNull())
                    continue;
                
                // TODO. Избавиться от получения компоненты.
                if (!character.TryGetComponent<IInteractor>(out IInteractor interactor))
                    continue;

                if (interactor.IsDead)
                    continue;
                        
                if (interactor == _playerInteractor)
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
        return (interactor is IMovable movable)
               && (_playerMovable != null)
               && !_playerMovable.IsUnityNull()
               && Vector3.Distance(movable.Position, _playerMovable.Position) <= interactDistance;
    }
}