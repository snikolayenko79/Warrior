using UnityEngine;

public class PlayerInputController : InputListener
{
    private IMovable moveTarget;

    public void SetTarget(IMovable movable)
    {
        moveTarget = movable;
    }
    
    public override void HandleInput(string actionName, InputContext context)
    {
        if (actionName == "Move")
        {
            moveTarget.MoveDirection = context.FloatValue == 0 ? Vector3.zero : moveTarget.Orientation;
        }
    }
}
