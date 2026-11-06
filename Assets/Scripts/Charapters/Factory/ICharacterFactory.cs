using UnityEngine;

public interface ICharacterFactory
{
    GameObject SpawnCharapter(GameObject prefab, Vector3 position, Quaternion rotation);
    void DespawnCharacter(Character character);
}
