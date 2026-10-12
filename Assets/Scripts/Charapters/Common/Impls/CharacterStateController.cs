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
        entity.StateBehaviours.Add(stateBehaviour);
        stateBehaviour.StateStart(entity);
    }

    public void StateEnd(ICharapterState entity, ICharacterStateBehaviour stateBehaviour)
    {
        stateBehaviour.StateEnd(entity);
        entity.StateBehaviours.Add(stateBehaviour);
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

            for (int j = entity.StateBehaviours.Count - 1; j >= 0; j--)
            {
                ICharacterStateBehaviour behaviour = entity.StateBehaviours[j];
                behaviour.StateUpdate(entity);
            }
            
            if (entity is IMovable movable)
            {
                moveController.UpdateMovement(movable);
            }
            if (entity is IAttacker attacker)
            {
                attackController.UpdateAttack(attacker);
            }
        }
    }

    private IMoveController _moveController;
    public IMoveController moveController
    {
        get => _moveController;
        set  => _moveController = value;
    }
    
    private IAttackController _attackController;
    public IAttackController attackController
    {
        get => _attackController;
        set  => _attackController = value;
    }

    void Awake()
    {
        TryGetComponent<IMoveController>(out _moveController);
        TryGetComponent<IAttackController>(out _attackController);
    }
    
    void Update()
    {
        UpdateStates();
    }
}
