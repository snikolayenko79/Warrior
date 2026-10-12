using UnityEngine;

public interface ICharacterStateController
{
    public void RegisterEntity(ICharapterState characterState);
    public void UnregisterEntity(ICharapterState characterState);
    void StateStart(ICharapterState entity, ICharacterStateBehaviour stateBehaviour);
    void StateEnd(ICharapterState entity, ICharacterStateBehaviour stateBehaviour);
    IMoveController moveController  { get; set; }
    IAttackController attackController  { get; set; }
    ISpellController spellController  { get; set; }
}