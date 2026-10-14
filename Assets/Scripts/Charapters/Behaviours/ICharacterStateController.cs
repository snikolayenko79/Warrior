using UnityEngine;

public interface ICharacterStateController
{
    public void RegisterEntity(ICharapterState characterState);
    public void UnregisterEntity(ICharapterState characterState);
    void StateStart(ICharapterState entity, IBehaviour stateBehaviour);
    void StateEnd(ICharapterState entity, IBehaviour stateBehaviour);
    IMoveController moveController  { get; set; }
    IAttackController attackController  { get; set; }
    ISpellController spellController  { get; set; }
}