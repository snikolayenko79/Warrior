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
    
    public abstract void StateStart(ICharapterState characterState);
    public abstract void StateUpdate(ICharapterState characterState);
    public abstract void StateEnd(ICharapterState characterState);
}