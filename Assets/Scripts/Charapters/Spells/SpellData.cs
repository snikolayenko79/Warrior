using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Data", menuName = "Spells/Data")]
public class SpellDtata : ScriptableObject
{
    public string spellName;
    public float cooldown;
    public int baseDamage;
    public GameObject projectilePrefab; // Если нужен снаряд
    public float speed;
    
    // Здесь могут быть ссылки на эффекты, звуки и т.д.
}