using UnityEngine;
using System.Collections.Generic;

public class CharacterStateController : MonoBehaviour, ICharacterStateController
{
    [SerializeField] List<CharacterStateBehaviourBase> statesBehaviours = new List<CharacterStateBehaviourBase>();
    
    protected List<ICharapterState> Entities = new List<ICharapterState>();
    
    public void RegisterEntity(ICharapterState characterState) => Entities.Add(characterState);
    public void UnregisterEntity(ICharapterState characterState) => Entities.Remove(characterState);

    public void StateStart(ICharapterState entity, ICharacterStateBehaviour stateBehaviour)
    {
        if (entity.CurrentStateBehaviour != null)
            entity.CurrentStateBehaviour.StateEnd(entity);

        entity.CurrentStateBehaviour = stateBehaviour;
        entity.CurrentStateBehaviour.StateStart(entity);
    }

    public void StateEnd(ICharapterState entity, ICharacterStateBehaviour stateBehaviour)
    {
        if (entity.CurrentStateBehaviour != null)
        {
            entity.CurrentStateBehaviour.StateEnd(entity);
            entity.CurrentStateBehaviour = null;
        }
    }

    void UpdateStates()
    {
        for (int i = Entities.Count - 1; i >= 0; i--)
        {
            ICharapterState entity = Entities[i];
            
            if (entity == null || entity.IsDead)
            {
                Entities.RemoveAt(i);
                continue;
            }
            
            if (entity.CurrentStateBehaviour != null)
            {
                entity.CurrentStateBehaviour.StateUpdate(entity);
            }
            
            if (entity is IMovable movable)
            {
                characterMoveController.UpdateMovement(movable);
            }
        }
    }

    private ICharacterMoveController _characterMoveController;
    public ICharacterMoveController characterMoveController
    {
        get => _characterMoveController;
        set  => _characterMoveController = value;
    }

    void Awake()
    {
        TryGetComponent<ICharacterMoveController>(out _characterMoveController);
    }
    
    void Update()
    {
        UpdateStates();
    }
}
