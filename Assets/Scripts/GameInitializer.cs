using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private NewInputEventSource MyInputSource;
    [SerializeField] private InputListener MyPlayerInputListener;
    private InputRouter inputRouter;
    
    void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MyInputSource);
        MyPlayerInputListener.Initialize(inputRouter);
    }
}