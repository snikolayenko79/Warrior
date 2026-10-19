using UnityEngine;
using Zenject;

public class Button_Start : MonoBehaviour
{
    private GameSceneLoader _sceneLoader;

    [Inject]
    public void Construct(GameSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OnClick()
    {
        _sceneLoader.LoadScene("World1");
    }
}