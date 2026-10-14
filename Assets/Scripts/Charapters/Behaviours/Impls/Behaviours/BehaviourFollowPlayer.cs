using UnityEngine;
using System.Collections.Generic;

public class BehaviourFollowPlayer : MovableBehaviourBase
{
    public List<Player> players = new List<Player>();
    public float startFollowDistanceMin = 0.7f;
    public float startFollowDistanceMax = 2.0f;
    public float stopFollowDistance = 0.5f;
    
    public override void StateStart(ICharapterState characterState)
    {
    }

    protected override void MovableUpdate(IMovable movable)
    {
        foreach (var player in  players)
        {
            float dist = Vector3.Distance(player.Position, movable.Position);
            
            if (dist <= stopFollowDistance)
            {
                movable.CurrentPath = null;
                break;
            }
            else if (dist <= startFollowDistanceMax && dist >= startFollowDistanceMin)
            {
                if (!(movable.CurrentPath is FollowTargetPath))
                    movable.CurrentPath = new FollowTargetPath(player);
                
                break;
            }
        }
    }
    
    public override void StateEnd(ICharapterState characterState)
    {
    }
}
