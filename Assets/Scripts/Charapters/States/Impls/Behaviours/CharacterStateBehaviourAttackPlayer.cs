using UnityEngine;
using System.Collections.Generic;

public class CharacterStateBehaviourAttackPlayer : AttackerBehaviourBase
{
    public List<Player> players = new List<Player>();
    public float attackDistance = 0.3f;
    
    public override void StateStart(ICharapterState characterState)
    {
        
    }
    
    protected override void AttackerUpdate(IAttacker attacker)
    {
        if (!attacker.IsAttacking && !attacker.IsDead)
        {
            foreach (var player in  players)
            {
                if (player.IsDead)
                    continue;
                
                if (Vector3.Distance(attacker.Position, player.Position) <= attackDistance)
                {
                    attacker.IsAttacking = true;
                    break;
                }
            }
        }
    }
    
    public override void StateEnd(ICharapterState characterState)
    {
    }
}