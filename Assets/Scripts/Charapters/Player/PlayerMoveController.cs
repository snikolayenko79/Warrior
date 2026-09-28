using System;
using UnityEngine;

public class PlayerMoveController : InputListener
{
    private IMovable moveTarget;

    public void SetTarget(IMovable _movable)
    {
        moveTarget = _movable;
    }
    
    public override void HandleInput(string logicalActionName, InputContext context)
    {
        if (logicalActionName == "Move")
        {
            moveTarget.IsMoving = context.FloatValue != 0;
        }
        // else if (logicalActionName == "Spell")
        // {
        //     if (context.FloatValue == 0) // TODO перенести в InputRouter. Записывать в конфиг для логического действия.
        //         spellCaster.Spell();
        // }
    }

    protected void Update()
    {
        if (moveTarget is {IsMoving: true})
        {
            moveTarget.Position += moveTarget.Orientation * moveTarget.MoveSpeed * Time.deltaTime;
        }
    }
}
