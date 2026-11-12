using UnityEngine;

public interface IBehaviour
{
    void StateStart(ICharacter character);
    void StateUpdate(ICharacter character);
    void StateEnd(ICharacter character);
}