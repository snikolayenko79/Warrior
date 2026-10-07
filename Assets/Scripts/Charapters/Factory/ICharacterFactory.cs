using UnityEngine;

public interface ICharacterFactory
{
    // Спавнит персонажа из пула, сбрасывает его состояние и регистрирует в системе контроллеров.
    GameObject SpawnCharapter(GameObject prefab, Vector3 position, Quaternion rotation);
}
