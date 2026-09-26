using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private NewInputSystemSource_CustomActions MuInputSource;
    [SerializeField] private Player MyPlayer;
    private InputRouter inputRouter;
    
    void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MuInputSource);
        
        if (MyPlayer)
            MyPlayer.Initialize(inputRouter);
    }
}
