using UnityEngine;
using Zenject;

public abstract class BehaviourBase : MonoBehaviour, IBehaviour
{
    protected IGameManager _gameManager;
    
    [Inject]
    public void Construct(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public abstract void StateStart(ICharacter character);
    public abstract void StateUpdate(ICharacter character);
    public abstract void StateEnd(ICharacter character);
}