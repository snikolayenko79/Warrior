using TMPro;
using UnityEngine;
using Zenject;

public class UIViewDialogue : UIView
{
    public TextMeshProUGUI textControl;
    public UITrianglePointer trianglePointer;
    public RectTransform trianglePointerPos;
    public Canvas canvas;
    
    private string _text;
    private IDialogueActor _currentActor;
    
    public override void OnCreate(params object[] args)
    {
        _text = args[0] as string;
        _currentActor = args[1] as IDialogueActor;
    }
    
    void Start()
    {
        if  (textControl == null)
            Debug.LogError("[UIViewDialogue] На префабе диалогов не выбран компонент для текста!");
        else
        {
            textControl.text = _text;
            UpdatePointer(_currentActor);
        }
    }
    
    public void UpdatePointer(IDialogueActor currentActor)
    {
        if (currentActor == null || currentActor.SpeechBubbleAnchor == null)
        {
            trianglePointer.gameObject.SetActive(false);
            return;
        }

        Camera activeCamera = Camera.main; 

        if (activeCamera == null)
            return;

        trianglePointer.gameObject.SetActive(true);
        
        // Позиция треугольника на диалоговом окне
        Vector3 uiBottomCenterWorld = trianglePointerPos.position;
        Vector2 startScreenPos;
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // В Overlay режиме мировые координаты UI и есть экранные пиксели
            startScreenPos = uiBottomCenterWorld;
        }
        else
        {
            // В Camera режиме переводим через камеру интерфейса
            Camera uiCamera = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
            startScreenPos = uiCamera.WorldToScreenPoint(uiBottomCenterWorld);
        }

        // Позиция персонажа, куда показываает треугольник
        Vector3 targetWorldPos = currentActor.SpeechBubbleAnchor;
        Vector2 endScreenPos = activeCamera.WorldToScreenPoint(targetWorldPos); 
        Vector3 camToSpeaker = targetWorldPos - activeCamera.transform.position;
        if (Vector3.Dot(camToSpeaker, activeCamera.transform.forward) < 0)
        {
            trianglePointer.gameObject.SetActive(false);
            return;
        }

        // Сам треугольный указатель
        trianglePointer.SetPositions(startScreenPos, endScreenPos);
    }
}