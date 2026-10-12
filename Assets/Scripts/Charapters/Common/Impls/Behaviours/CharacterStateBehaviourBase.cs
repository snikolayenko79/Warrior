using UnityEngine;

public abstract class CharacterStateBehaviourBase : MonoBehaviour, ICharacterStateBehaviour
{
    public abstract void StateStart(ICharapterState characterState);
    public abstract void StateUpdate(ICharapterState characterState);
    public abstract void StateEnd(ICharapterState characterState);
}
