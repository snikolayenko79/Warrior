using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private NewInputEventSource MyInputSource;
    [SerializeField] private InputListener MyPlayerInputListener;
    [SerializeField] private InputListener MyPlayerInputListener2;
    private InputRouter inputRouter;
    
    async void Start()
    {
        inputRouter = new  InputRouter();
        inputRouter.Initialize(MyInputSource);
        MyPlayerInputListener.Initialize(inputRouter);
        MyPlayerInputListener2.Initialize(inputRouter);
        
        int a = Profile.GetVar<int>("a");
        Debug.Log("Loaded = " + a);
        Profile.SetVar("a", a + 1);
    }
}