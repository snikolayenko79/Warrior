using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIViewText : UIView
{
    public TextMeshProUGUI textControl;
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    [Tooltip("Время показа текста")]
    public float standTime = 1.0f;
    [Tooltip("Время фейда исчезновения")]
    public float fadeoutTime = 0.8f;
    [Tooltip("Скорость перемещения вверх в конце")]
    public float UpSpeed = 300.0f;
    
    private string _text;
    private Vector2 _screenPos;
    
    public override void OnCreate(params object[] args)
    {
        _text = args[0] as string;
        
        float x = args[1] is float fX ? fX : 0f;
        float y = args[2] is float fY ? fY : 0f;
        
        _screenPos = new Vector2(x, y);
    }
    
    IEnumerator Start()
    {
        if  (textControl == null)
            Debug.LogError("[UIViewText] На префабе текстов не выбран компонент для текста!");
        else
        {
            textControl.text = _text;
            UpdatePosition(_screenPos);
        }
        
        yield return new WaitForSeconds(standTime);

        float fadeCounter = 0;
        while (fadeCounter <= fadeoutTime)
        {
            fadeCounter += Time.deltaTime;
            
            _screenPos.y += UpSpeed * Time.deltaTime;
            UpdatePosition(_screenPos);
            
            canvasGroup.alpha = Mathf.Lerp(1, 0, fadeCounter/fadeoutTime);
            
            yield return null;
        }
        
        Object.Destroy(gameObject);
    }

    public void UpdatePosition(Vector2 screenPos)
    {
        if (canvas == null)
        {
            Debug.LogError("[UIViewText] Объект должен находиться внутри Canvas!");
            return;
        }

        // TODO. Пока только режим оверлей.
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // В режиме Overlay мы просто передаем экранную позицию (пиксели) напрямую
            textControl.rectTransform.anchoredPosition = screenPos;
        }
        // else
        // {
        //     // Для ScreenSpaceCamera или WorldSpace:
        //     Camera uiCamera = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
        //
        //     // Получаем RectTransform Canvas'а (он является родителем или самим этим объектом)
        //     RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        //     Vector2 localPos;
        //
        //     // Считаем позицию относительно Canvas
        //     if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out localPos))
        //     {
        //         rectTransform.anchoredPosition = localPos;
        //     }
        // }
    }
}
