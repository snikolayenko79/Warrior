using UnityEngine;
using System.Collections.Generic;

public class BehaviourStartFollowPlayer : MovableStateBehaviourBase
{
    public List<Player> players = new List<Player>();
    public float startFollowDistance = 2.0f;
    
    public override void StateStart(ICharapterState characterState)
    {
    }

    protected override void MovableUpdate(IMovable movable)
    {
        foreach (var player in  players)
        {
            if (Vector3.Distance(player.Position, movable.Position) <= startFollowDistance)
            {
                movable.CurrentPath = new FollowTargetPath(player);
                break;
            }
        }
    }
    
    public override void StateEnd(ICharapterState characterState)
    {
    }
}
