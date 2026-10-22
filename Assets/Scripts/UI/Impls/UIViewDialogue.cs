using TMPro;
using UnityEngine;
using Zenject;

public class UIViewDialogue : UIView
{
    public TextMeshProUGUI textControl;
    private string _text;
    
    void Start()
    {
        if  (textControl == null)
            Debug.LogError("[UIViewDialogue] На префабе диалогов не выбран компонент для текста!");
        else
        {
            textControl.text = _text;
        }
    }

    public override void Init(params object[] args)
    {
        _text = args[0] as string;
    }
}