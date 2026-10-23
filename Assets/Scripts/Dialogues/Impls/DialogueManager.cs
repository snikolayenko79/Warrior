using System.Linq;
using UnityEngine;
using Zenject;

public class DialogueManager : IDialogManager
{
    private IDialogue _currentDialogue;
    private IDialogueNode _currentNode;
    private int _currentNodeIndex;
    
    private IUIManager _uiManager;
    private GameObject _dialoguePrefab;

    [Inject]
    public void Construct(IUIManager uiManager, [Inject(Id = "DialoguePrefab")] GameObject dialoguePrefab)
    {
        _uiManager = uiManager;
        _dialoguePrefab = dialoguePrefab;
    }
    
    public IDialogue CurrentDialogue { get; }
    public IDialogueNode CurrentNode { get; }

    public void StartDialogue(IDialogue dialogue)
    {
        if ((dialogue == null) || !dialogue.DialogueNodes.Any())
            return;

        _currentDialogue = dialogue;
        _currentNode = dialogue.DialogueNodes.First();
        _currentNodeIndex = 0;

        DisplayCurrentNode();
    }

    public void DialogueNext()
    {
        if (_currentDialogue == null)
            return;
        
        _currentNodeIndex++;

        if (_currentNodeIndex < _currentDialogue.DialogueNodes.Count())
        {
            _currentNode = _currentDialogue.DialogueNodes.ElementAt(_currentNodeIndex);
            DisplayCurrentNode();
        }
        else
        {
            DialogueEnd();
        }
    }
    
    public void DialogueEnd()
    {
        // TODO. Убедиться, что закрываем именно окно диалога.
        _uiManager.CloseActiveView();
        
        _currentDialogue = null;
        _currentNode = null;
    }
    
    public bool IsDialogueRunning => _currentDialogue != null;

    private void DisplayCurrentNode()
    {
        _uiManager.OpenPopup(_dialoguePrefab, _currentNode.DialogueText);
    }
}