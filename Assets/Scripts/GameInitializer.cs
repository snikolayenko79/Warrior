using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private NewInputEventSource MyInputSource;
    [SerializeField] private PlayerMoveController MyPlayerMoveController;
    [SerializeField] private PlayerRotateController MyPlayerRotateController;
    [SerializeField] private Player MyPlayer;
    private InputRouter inputRouter;
    
    void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MyInputSource);

        if (MyPlayerMoveController)
        {
            MyPlayerMoveController.Initialize(inputRouter);
            MyPlayerMoveController.SetTarget(MyPlayer);
        }
        
        if (MyPlayerRotateController)
        {
            MyPlayerRotateController.Initialize(inputRouter);
            MyPlayerRotateController.SetTarget(MyPlayer);
        }
    }
}
