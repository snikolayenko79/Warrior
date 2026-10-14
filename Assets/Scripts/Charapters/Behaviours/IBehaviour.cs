using UnityEngine;

public interface IBehaviour
{
    void StateStart(ICharapterState characterState);
    void StateUpdate(ICharapterState characterState);
    void StateEnd(ICharapterState characterState);
}