using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private NewInputEventSource MuInputSource;
    [SerializeField] private PlayerInputController MyPlayerController;
    [SerializeField] private Player MyPlayer;
    private InputRouter inputRouter;
    
    void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MuInputSource);

        if (MyPlayerController)
        {
            MyPlayerController.Initialize(inputRouter);
            MyPlayerController.SetTarget(MyPlayer);
        }
    }
}
