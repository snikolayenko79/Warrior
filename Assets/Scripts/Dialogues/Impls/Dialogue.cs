using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues/Dialogue")]
public class Dialogue : ScriptableObject, IDialogue
{
    [SerializeField] private string dialogueId;
    [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    public string DialogueID => dialogueId;
    public IEnumerable<IDialogueNode> DialogueNodes => dialogueNodes;
}