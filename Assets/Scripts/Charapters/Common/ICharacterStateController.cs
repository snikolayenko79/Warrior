using UnityEngine;

public interface ICharacterStateController
{
    public void RegisterEntity(ICharapterState characterState);
    public void UnregisterEntity(ICharapterState characterState);
    void StateStart(ICharapterState entity, ICharacterStateBehaviour stateBehaviour);
    void StateEnd(ICharapterState entity, ICharacterStateBehaviour stateBehaviour);
    ICharacterMoveController characterMoveController  { get; set; }
}