using UnityEngine;

public abstract class AttackerBehaviourBase : BehaviourBase
{
    public override void StateUpdate(ICharacter character)
    {
        if (character is IAttacker attacker)
        {
            AttackerUpdate(attacker);
        }
    }

    protected abstract void AttackerUpdate(IAttacker attacker);
}