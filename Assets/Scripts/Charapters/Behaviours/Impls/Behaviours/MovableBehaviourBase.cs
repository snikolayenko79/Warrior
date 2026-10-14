using UnityEngine;

public abstract class MovableBehaviourBase : BehaviourBase
{
    public override void StateUpdate(ICharapterState characterState)
    {
        if (characterState is IMovable movable)
        {
            MovableUpdate(movable);
        }
    }

    protected abstract void MovableUpdate(IMovable movable);
}
