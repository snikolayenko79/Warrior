using UnityEngine;
using Zenject;

public class Button_Start : MonoBehaviour
{
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OnClick()
    {
        _sceneLoader.LoadScene("World1");
    }
}