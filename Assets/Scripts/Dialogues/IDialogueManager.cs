using UnityEngine;

public interface IDialogManager
{
    IDialogue CurrentDialogue { get; }
    IDialogueNode CurrentNode { get; }
    IDialogueActorsRegistry ActorsRegistry { get; }
    
    void StartDialogue(IDialogue dialogue);
    void DialogueNext();
    void DialogueEnd();
    bool IsDialogueRunning { get; }
}