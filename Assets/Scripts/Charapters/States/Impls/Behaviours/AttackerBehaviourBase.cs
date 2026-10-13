using UnityEngine;

public abstract class AttackerBehaviourBase : CharacterStateBehaviourBase
{
    public override void StateUpdate(ICharapterState characterState)
    {
        if (characterState is IAttacker attacker)
        {
            AttackerUpdate(attacker);
        }
    }

    protected abstract void AttackerUpdate(IAttacker attacker);
}