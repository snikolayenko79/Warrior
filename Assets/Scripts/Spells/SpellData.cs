using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Spell Data", menuName = "Spells/Spell Data")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public float spellCost;
    public float spellDamage;
    public SpellBehaviour spellBehaviourPrefab;
}