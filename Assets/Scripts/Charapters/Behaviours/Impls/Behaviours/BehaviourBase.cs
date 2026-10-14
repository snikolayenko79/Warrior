using UnityEngine;

public abstract class BehaviourBase : MonoBehaviour, IBehaviour
{
    public abstract void StateStart(ICharapterState characterState);
    public abstract void StateUpdate(ICharapterState characterState);
    public abstract void StateEnd(ICharapterState characterState);
}