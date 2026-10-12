using System;
using UnityEngine;

public class PlayerMoveController : CharapterMoveController
{ 
    private IInputListener _inputListener;

    private bool _isInitialized;
    
    public void SetInputListener(IInputListener inputListener)
    {
        _inputListener  = inputListener;
        _inputListener.OnInputAction += OnInputAction;
    }
    
    protected virtual void OnEnable()
    {
        if (_isInitialized && _inputListener != null)
        {
            _inputListener.OnInputAction += OnInputAction;
        }
    }
    
    protected virtual void OnDisable()
    {
        if (_isInitialized && _inputListener != null)
        {
            _inputListener.OnInputAction -= OnInputAction;
        }
    }
    
    private void OnInputAction(string logicalActionName, InputContext context)
    {
        if (MovableEntities.Count <= 0)
            return;
        
        IMovable player = MovableEntities[0];
        
        if (player == null || player.IsDead)
            return;
        
        if (logicalActionName == "Move")
        {
            bool bMove= context.FloatValue != 0;
            
            if (bMove && !player.IsMoving)
                BeginMove(player);
            else if (!bMove && player.IsMoving)
                StopMove(player);
        }
        
        if (logicalActionName == "Rotate")
        {
            bool bRotate = context.FloatValue != 0;
            
            if (bRotate && !player.IsRotating)
                BeginRotate(player, context.FloatValue);
            else if (!bRotate && player.IsRotating)
                StopRotate(player);
        }
    }
}