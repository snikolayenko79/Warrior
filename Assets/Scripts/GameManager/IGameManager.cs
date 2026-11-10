using UnityEngine;
using System.Collections.Generic;

public interface IGameManager
{
    GameObject CreateCharacter(string name, GameObject prefab, Vector3 position, Quaternion rotation, SideType side);
    IReadOnlyList<GameObject> GetCharactersBySide(SideType side);
    IReadOnlyList<GameObject> GetAllCharacters();

    void SetGameplayLocked(bool bLock);
    bool IsGameplayLocked();
}