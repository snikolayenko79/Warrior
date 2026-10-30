using UnityEngine;

public class GameManager : IGameManager
{
    ICharapterState player;

    public ICharapterState Player
    {
        get => player;
        set => player = value;
    }
}