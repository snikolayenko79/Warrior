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
    private DiContainer _container;

    [Inject]
    public void Construct(IUIManager uiManager, [Inject(Id = "DialoguePrefab")] GameObject dialoguePrefab, DiContainer container)
    {
        _uiManager = uiManager;
        _dialoguePrefab = dialoguePrefab;
        _container  = container;
    }

    public IDialogue CurrentDialogue => _currentDialogue;
    public IDialogueNode CurrentNode => _currentNode;
    public IDialogueActorsRegistry ActorsRegistry => _container.Resolve<IDialogueActorsRegistry>();

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
            // TODO. Убедиться, что закрываем именно окно диалога.
            _uiManager.CloseActiveWindow();
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
        _uiManager.CloseActiveWindow();
        
        _currentDialogue = null;
        _currentNode = null;
    }
    
    public bool IsDialogueRunning => _currentDialogue != null;

    private void DisplayCurrentNode()
    {
        // Ресолвим, именно так, а не через конструктор. Т.к. сам менеджер в глоб. контексте, а регистр - в локальном для сцены.
        IDialogueActor currentActor = null;
        IDialogueActorsRegistry registry = _container.Resolve<IDialogueActorsRegistry>();
        if (registry != null)
            currentActor = registry.GetActor(_currentNode.SpeakerId);
        
        _uiManager.OpenWindow(_dialoguePrefab, _currentNode.DialogueText, currentActor);
    }
}