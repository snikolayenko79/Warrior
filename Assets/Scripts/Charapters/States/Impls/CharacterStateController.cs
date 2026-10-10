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
        //Debug.Log("5");
        if (entity.CurrentStateBehaviour != null)
            entity.CurrentStateBehaviour.StateEnd(entity);

        entity.CurrentStateBehaviour = stateBehaviour;
        entity.CurrentStateBehaviour.StateStart(entity);
        
        //Debug.Log(entity + ", " + nameof(entity.CurrentStateBehaviour));
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
            //Debug.Log("2");
            if (entity == null || entity.IsDead)
            {
                Entities.RemoveAt(i);
                continue;
            }
            //Debug.Log("3");
            //Debug.Log(entity + ", " + nameof(entity.CurrentStateBehaviour));
            if (entity.CurrentStateBehaviour != null)
            {
                //Debug.Log("4");
                entity.CurrentStateBehaviour.StateUpdate(entity);
            }
        }
    }
    
    void Update()
    {
        //Debug.Log("1");
        UpdateStates();
    }
}
