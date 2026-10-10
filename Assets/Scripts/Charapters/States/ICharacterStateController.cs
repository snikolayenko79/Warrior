using UnityEngine;

public interface ICharacterStateController
{
    void StateStart(ICharapterState entity, ICharacterStateBehaviour stateBehaviour);
    void StateEnd(ICharapterState entity, ICharacterStateBehaviour stateBehaviour);
}