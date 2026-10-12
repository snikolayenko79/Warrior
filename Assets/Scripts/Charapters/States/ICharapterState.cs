using UnityEngine;
using System.Collections.Generic;

public interface ICharapterState
{
    List<ICharacterStateBehaviour> StateBehaviours { get; set; }
    bool IsDead { get; }
}
