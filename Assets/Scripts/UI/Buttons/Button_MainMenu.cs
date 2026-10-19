using UnityEngine;
using Zenject;

public class Button_MainMenu : MonoBehaviour
{
    private GameSceneLoader _sceneLoader;

    [Inject]
    public void Construct(GameSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OnClick()
    {
        Time.timeScale = 1;
        _sceneLoader.LoadScene("MainMenu");
    }
}