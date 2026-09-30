using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private NewInputEventSource MyInputSource;
    [SerializeField] private InputListener MyPlayerInputListener;
    
    [SerializeField] private PlayerMoveController MyPlayerMoveController;
    [SerializeField] private PlayerSpellController MyPlayerSpellController;
    [SerializeField] private Player MyPlayer;
    private InputRouter inputRouter;
    
    [SerializeField] private AIMoveController MyEnemyMoveController;
    [SerializeField] private Enemy MyEnemy;
    
    void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MyInputSource);
        MyPlayerInputListener.Initialize(inputRouter);

        if (MyPlayerMoveController != null)
        {
            MyPlayerMoveController.SetInputListener(MyPlayerInputListener);
            MyPlayerMoveController.SetTarget(MyPlayer);
        }
        
        if (MyPlayerSpellController != null)
        {
            MyPlayerSpellController.SetInputListener(MyPlayerInputListener);
            MyPlayerSpellController.SetCaster(MyPlayer);
        }
        
        if (MyEnemyMoveController != null)
        {
            MyEnemyMoveController.SetTarget(MyEnemy);
            MyEnemyMoveController.SetAttackable(MyPlayer);
        }
    }
}