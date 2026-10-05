using UnityEngine;

public class PlayerAttackController : AttackController
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
        if (Attacker == null ||  Attacker.IsDead)
            return;
        
        if (logicalActionName == "Attack" && context.FloatValue == 0)
        {
            DoAttack();
        }
    }
}
