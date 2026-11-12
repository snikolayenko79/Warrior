using UnityEngine;

public abstract class MovableBehaviourBase : BehaviourBase
{
    public override void StateUpdate(ICharacter character)
    {
        if (character is IMovable movable)
        {
            MovableUpdate(movable);
        }
    }

    protected abstract void MovableUpdate(IMovable movable);
}
