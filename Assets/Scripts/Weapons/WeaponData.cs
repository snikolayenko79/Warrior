using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float weaponCost;
    public float weaponDamage;
    public GameObject weaponPrefab;
    public WeaponBehaviour weaponBehaviourPrefab;
}