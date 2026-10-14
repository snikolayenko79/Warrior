using UnityEngine;
using System.Collections.Generic;

public interface ICharapterState
{
    List<IBehaviour> StateBehaviours { get; set; }
    Vector3 Position { get; set; }
    bool IsDead { get; }
}
