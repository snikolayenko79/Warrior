using UnityEngine;

public class BehaviourPlayerInput : BehaviourBase
{
    private IMovable _movable;
    private IAttacker _attacker;
    private ISpellCaster _spellcaster;
    private IInputListener _inputListener;
    
    public void SetInputListener(IInputListener inputListener)
    {
    }
    
    public override void StateStart(ICharapterState characterState)
    {
        _movable = characterState as IMovable;
        _attacker = characterState as IAttacker;
        _spellcaster  = characterState as ISpellCaster;
        
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
        if (logicalActionName == "Move")
        {
            if (_movable == null || _movable.IsDead)
                return;
            
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
            if (_movable == null || _movable.IsDead)
                return;
            
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
        
        if (logicalActionName == "Attack" && context.FloatValue == 0)
        {
            if (_attacker == null || _attacker.IsDead)
                return;

            if (!_attacker.IsAttacking)
            {
                _attacker.IsAttacking = true;
            }
        }
        
        if (logicalActionName == "Spell" && context.FloatValue == 0)
        {
            if (_spellcaster == null || _spellcaster.IsDead || _spellcaster.AvailableSpells.Count == 0)
                return;

            if (!_spellcaster.IsSpellCast)
            {
                _spellcaster.IsSpellCast = true;
                _spellcaster.SelectedSpell = _spellcaster.AvailableSpells[0];
            }
        }
    }
}