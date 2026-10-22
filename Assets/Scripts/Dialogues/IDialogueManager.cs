using UnityEngine;

public interface IDialogManager
{
    IDialogue CurrentDialogue { get; }
    IDialogueNode CurrentNode { get; }
    
    void StartDialogue(IDialogue dialogue);
    void DialogueNext();
    void DialogueEnd();
    bool IsDialogueRunning { get; }
}