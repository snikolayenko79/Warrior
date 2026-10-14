using UnityEngine;
using System.Collections.Generic;

public class CharacterStateController : MonoBehaviour, ICharacterStateController
{
    [SerializeField] List<BehaviourBase> statesBehaviours = new List<BehaviourBase>();
    
    protected List<ICharapterState> Entities = new List<ICharapterState>();
    
    public void RegisterEntity(ICharapterState characterState) => Entities.Add(characterState);
    public void UnregisterEntity(ICharapterState characterState) => Entities.Remove(characterState);

    public void StateStart(ICharapterState entity, IBehaviour stateBehaviour)
    {
        entity.StateBehaviours.Add(stateBehaviour);
        stateBehaviour.StateStart(entity);
    }

    public void StateEnd(ICharapterState entity, IBehaviour stateBehaviour)
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
                IBehaviour behaviour = entity.StateBehaviours[j];
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
            if (entity is ISpellCaster caster)
            {
                spellController.UpdateSpellCast(caster);
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
    
    private ISpellController _spellController;
    public ISpellController spellController
    {
        get => _spellController;
        set  => _spellController = value;
    }

    void Awake()
    {
        TryGetComponent<IMoveController>(out _moveController);
        TryGetComponent<IAttackController>(out _attackController);
        TryGetComponent<ISpellController>(out _spellController);
    }
    
    void Update()
    {
        UpdateStates();
    }
}
