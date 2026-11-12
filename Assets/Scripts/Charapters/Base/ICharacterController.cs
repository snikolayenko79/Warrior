using UnityEngine;

public interface ICharacterController
{
    public void RegisterEntity(ICharacter entity);
    public void UnregisterEntity(ICharacter entity);
    void StateStart(ICharacter entity, IBehaviour behaviour);
    void StateEnd(ICharacter entity, IBehaviour behaviour);
    IMoveController moveController  { get; set; }
    IAttackController attackController  { get; set; }
    ISpellController spellController  { get; set; }
}