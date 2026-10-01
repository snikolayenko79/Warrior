using UnityEngine;

public class PlayerSpellController : SpellController
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
        if (logicalActionName == "Spell" && context.FloatValue == 0)
        {
            if (SpellCaster != null && SpellCaster.CurrentSpell != null)
            {
                SpellCaster.Spell();

                SpellBehaviour instantiatedSpell = Instantiate(SpellCaster.CurrentSpell.spellBehaviourPrefab, this.transform);
                instantiatedSpell.Execute(SpellCaster);
            }
        }
    }
}
