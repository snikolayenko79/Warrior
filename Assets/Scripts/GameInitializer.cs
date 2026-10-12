using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private NewInputEventSource MyInputSource;
    [SerializeField] private InputListener MyPlayerInputListener;
    
    [SerializeField] private PlayerSpellController MyPlayerSpellController;
    [SerializeField] private PlayerAttackController myPlayerAttackController;
    [SerializeField] private Player MyPlayer;
    private InputRouter inputRouter;
    
    void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MyInputSource);
        MyPlayerInputListener.Initialize(inputRouter);
        
        if (myPlayerAttackController != null)
        {
            myPlayerAttackController.SetInputListener(MyPlayerInputListener);
            myPlayerAttackController.SetAttacker(MyPlayer);
        }
        
        if (MyPlayerSpellController != null)
        {
            MyPlayerSpellController.SetInputListener(MyPlayerInputListener);
            MyPlayerSpellController.SetCaster(MyPlayer);
        }
    }
}