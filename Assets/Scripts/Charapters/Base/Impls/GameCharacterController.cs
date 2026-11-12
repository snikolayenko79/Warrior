using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GameCharacterController : MonoBehaviour, ICharacterController
{
    // TODO. Нужны?
    [SerializeField] List<BehaviourBase> behaviours = new List<BehaviourBase>();
    
    protected List<ICharacter> Entities = new List<ICharacter>();
    
    public void RegisterEntity(ICharacter character) => Entities.Add(character);
    public void UnregisterEntity(ICharacter character) => Entities.Remove(character);

    public void StateStart(ICharacter entity, IBehaviour stateBehaviour)
    {
        entity.Behaviours.Add(stateBehaviour);
        stateBehaviour.StateStart(entity);
    }

    public void StateEnd(ICharacter entity, IBehaviour stateBehaviour)
    {
        stateBehaviour.StateEnd(entity);
        entity.Behaviours.Remove(stateBehaviour);
    }

    void UpdateStates()
    {
        for (int i = Entities.Count - 1; i >= 0; i--)
        {
            ICharacter entity = Entities[i];
            
            if (entity == null || entity.IsDead)
            {
                Entities.RemoveAt(i);
                continue;
            }

            for (int j = entity.Behaviours.Count - 1; j >= 0; j--)
            {
                IBehaviour behaviour = entity.Behaviours[j];
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
