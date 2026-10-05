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
        if (MoveTarget == null || MoveTarget.IsDead)
            return;
        
        if (logicalActionName == "Move")
        {
            bool bMove= context.FloatValue != 0;
            
            if (bMove && !MoveTarget.IsMoving)
                BeginMove();
            else if (!bMove && MoveTarget.IsMoving)
                StopMove();
        }
        
        if (logicalActionName == "Rotate")
        {
            bool bRotate = context.FloatValue != 0;
            
            if (bRotate && !MoveTarget.IsRotating)
                BeginRotate(context.FloatValue);
            else if (!bRotate && MoveTarget.IsRotating)
                StopRotate();
        }
    }

    protected new void Update()
    {
        base.Update();
    }
}