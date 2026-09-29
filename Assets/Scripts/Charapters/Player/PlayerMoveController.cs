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
        if (logicalActionName == "Move")
        {
            MoveTarget.IsMoving = context.FloatValue != 0;
        }
        
        if (logicalActionName == "Rotate")
        {
            MoveTarget.RotationDirection = context.FloatValue;
        }
    }

    protected new void Update()
    {
        base.Update();
    }
}