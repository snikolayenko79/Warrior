using UnityEngine;

public interface IDialogueActor
{
    string ActorId { get; }
    string DisplayName { get; }
    Vector3 SpeechBubbleAnchor { get; }
}