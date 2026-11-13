using UnityEngine;
using System.Collections.Generic;

public interface ICharacter
{
    List<IBehaviour> Behaviours { get; set; }
    Vector3 Position { get; set; }
    bool IsDead { get; }
    // TODO. Добавить side?
}