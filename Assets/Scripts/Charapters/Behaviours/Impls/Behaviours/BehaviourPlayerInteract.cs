using UnityEngine;
using Zenject;

public class BehaviourPlayerInteract : BehaviourBase
{
    public float interactDistance = 0.3f;
    
    private IGameManager _gameManager;

    [Inject]
    public void Construct(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private IMovable _player;
    private IInputListener _inputListener;
    
    public void SetInputListener(IInputListener inputListener)
    {
    }
    
    public override void StateStart(ICharapterState characterState)
    {
        _player = characterState as IMovable;
        
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
            if (_player == null || _player.IsDead)
                return;
            
            foreach (var character in _gameManager.GetCharactersBySide(SideType.Friendly))
            {
                if (Vector3.Distance(character.transform.position, _player.Position) <= interactDistance)
                {
                    InteractWith(character);
                    break;
                }
            }
        }
    }

    private void InteractWith(GameObject character)
    {
        if (character.name == "Villager")
        {
            InteractWithVillager(character);
        }
        else if (character.name == "Seller")
        {
            InteractWithSeller(character);
        }
    }
    
    private void InteractWithVillager(GameObject villager)
    {
        Debug.Log("InteractWithVillager");
    }
    
    private void InteractWithSeller(GameObject seller)
    {
        Debug.Log("InteractWithSeller");
    }
}