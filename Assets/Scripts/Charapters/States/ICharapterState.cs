using UnityEngine;
using System.Collections.Generic;

public interface ICharapterState
{
    List<ICharacterStateBehaviour> StateBehaviours { get; set; }
    Vector3 Position { get; set; }
    bool IsDead { get; }
}
