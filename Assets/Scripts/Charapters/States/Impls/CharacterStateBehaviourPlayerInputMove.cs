using UnityEngine;

public class CharacterStateBehaviourPlayerInputMove : CharacterStateBehaviourMoveBase
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
        
    private void OnInputAction(string logicalActionName, InputContext context)
    {
        if (_movable == null || _movable.IsDead)
            return;
                
        if (logicalActionName == "Move")
        {
            bool bMove= context.FloatValue != 0;

            if (bMove && !_movable.IsMoving)
            {
                BeginMove(_movable);
            }
            else if (!bMove && _movable.IsMoving)
            {
                StopMove((_movable));
            }
        }
            
        if (logicalActionName == "Rotate")
        {
            bool bRotate = context.FloatValue != 0;

            if (bRotate && !_movable.IsRotating)
            {
                BeginRotate(_movable, context.FloatValue);
            }
            else if (!bRotate && _movable.IsRotating)
            {
                StopRotate(_movable);
            }
        }
    }
}