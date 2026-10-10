using UnityEngine;

public interface ICharapterState
{
    ICharacterStateBehaviour CurrentStateBehaviour { get; set; }
    bool IsDead { get; }
}
