using UnityEngine;

public interface ICharacterStateBehaviour
{
    void StateStart(ICharapterState characterState);
    void StateUpdate(ICharapterState characterState);
    void StateEnd(ICharapterState characterState);
}