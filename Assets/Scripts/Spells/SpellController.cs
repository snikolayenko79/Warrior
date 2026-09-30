using UnityEngine;

public class SpellController : MonoBehaviour
{
    protected ISpellCaster SpellCaster;

    public void SetCaster(ISpellCaster caster)
    {
        SpellCaster = caster;
    }
}
