using UnityEngine;

public class CoroutineRunner : MonoBehaviour 
{
    private void Awake()
    {
        // Гарантируем, что раннер корутин не уничтожится при смене сцен, 
        // если Zenject создаст его вне родительского ProjectContext
        DontDestroyOnLoad(gameObject);
    }
}