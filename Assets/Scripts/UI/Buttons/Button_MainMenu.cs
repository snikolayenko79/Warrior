using UnityEngine;
using Zenject;

public class Button_MainMenu : MonoBehaviour
{
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OnClick()
    {
        Time.timeScale = 1;
        _sceneLoader.LoadScene("MainMenu");
    }
}