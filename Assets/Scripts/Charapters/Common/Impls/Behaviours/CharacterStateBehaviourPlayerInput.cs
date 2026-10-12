using UnityEngine;

public class CharacterStateBehaviourPlayerInputMove : CharacterStateBehaviourBase
{
    private IMovable _movable;
    private IInputListener _inputListener;
    
    public void SetInputListener(IInputListener inputListener)
    {
        _inputListener  = inputListener;
        _inputListener.OnInputAction += OnInputAction;
    }
    
    public override void StateStart(ICharapterState characterState)
    {
        _movable = characterState as IMovable;
        
        TryGetComponent<IInputListener>(out _inputListener);

        if (_inputListener != null)
        {
            _inputListener.OnInputAction += OnInputAction;
        }
    }
    
    public override void StateEnd(ICharapterState characterState)
    {
        if (_inputListener != null)
        {
            _inputListener.OnInputAction -= OnInputAction;
        }
    }

    public override void StateUpdate(ICharapterState characterState)
    {
    }

    private void OnInputAction(string logicalActionName, InputContext context)
    {
        if (_movable == null || _movable.IsDead)
            return;
                
        if (logicalActionName == "Move")
        {
            bool bMove= context.FloatValue != 0;

            if (bMove && !_movable.IsMoving)
            {
                _movable.IsMoving = true;
                _movable.MoveSpeed = 0;
            }
            else if (!bMove && _movable.IsMoving)
            {
                _movable.IsMoving = false;
            }
        }
            
        if (logicalActionName == "Rotate")
        {
            bool bRotate = context.FloatValue != 0;

            if (bRotate && !_movable.IsRotating)
            {
                _movable.RotationDirection = context.FloatValue;
                _movable.RotationSpeed = 0;
            }
            else if (!bRotate && _movable.IsRotating)
            {
                _movable.RotationDirection = 0;
            }
        }
    }
}