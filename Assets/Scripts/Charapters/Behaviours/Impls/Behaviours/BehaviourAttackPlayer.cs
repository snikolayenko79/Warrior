using UnityEngine;
using System.Collections.Generic;

public class BehaviourAttackPlayer : AttackerBehaviourBase
{
    public List<Player> players = new List<Player>();
    public float attackDistance = 0.3f;
    
    const float BaseAttackChance = 1f;
    const float MaxAttackChance = 50f;
    const float PauseBetweenAttacks = 2f;
    
    public override void StateStart(ICharapterState characterState)
    {
        
    }
    
    protected override void AttackerUpdate(IAttacker attacker)
    {
        if (attacker.IsAttacking || attacker.IsDead)
            return;

        if (attacker.LockAttackCounter > 0)
        {
            attacker.LockAttackCounter -= Time.deltaTime;
            return;
        }
        
        foreach (var player in  players)
        {
            if (player.IsDead)
                continue;
                
            if (Vector3.Distance(attacker.Position, player.Position) <= attackDistance)
            {
                if (TryToAttack(attacker))
                {
                    attacker.LockAttackCounter = PauseBetweenAttacks;
                    if (attacker is IMovable movable)
                    {
                        RotateTowards(movable, player.Position);
                    }
                    break;
                }
            }
        }
    }
    
    private bool TryToAttack(IAttacker attacker)
    {
        float t = attacker.Experience / 100f;
        float currentChance = Mathf.Lerp(BaseAttackChance, MaxAttackChance, t);
        if (Random.value <= (currentChance / 100f))
        {
            attacker.IsAttacking = true;
            return  true;
        }

        return  false;
    }
    
    private void RotateTowards(IMovable target, Vector3 targetPos)
    {
        Vector3 directionToPlayer = targetPos - target.Position;
        directionToPlayer.y = 0;

        float forwardDot = Vector3.Dot(target.Orientation, directionToPlayer.normalized);
        float angle = Mathf.Acos(forwardDot) * Mathf.Rad2Deg;
        if (angle > 1)
        {
            target.Rotation += new Vector3(0, 0, angle);
        }
    }
    
    public override void StateEnd(ICharapterState characterState)
    {
    }
}