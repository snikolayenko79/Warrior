using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue node", menuName = "Dialogues/Dialogue node")]
public class DialogueNode : ScriptableObject, IDialogueNode
{
    [SerializeField] private string nodeId;
    [SerializeField] private string speakerId;
    [SerializeField] [TextArea] private string dialogueText;

    public string NodeId => nodeId;
    public string SpeakerId => speakerId;
    public string DialogueText => dialogueText;
}