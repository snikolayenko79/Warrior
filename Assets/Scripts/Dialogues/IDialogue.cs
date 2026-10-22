using UnityEngine;
using System.Collections.Generic;

public interface IDialogue
{
    string DialogueID { get; }
    IEnumerable<IDialogueNode> DialogueNodes { get; }
}

public interface IDialogueNode
{
    string NodeId { get; }
    string SpeakerId { get; }
    string DialogueText { get; }
}