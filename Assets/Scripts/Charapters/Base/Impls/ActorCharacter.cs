using System.Reflection.Emit;
using UnityEngine;
using Zenject;

public class ActorCharacter : Character, IDialogueActor
{
    public string dialogueActorName = "";
    public Transform speechBubbleAnchor;
    
    public string ActorId => dialogueActorName;
    public string DisplayName => dialogueActorName;
    public Vector3 SpeechBubbleAnchor => speechBubbleAnchor != null? speechBubbleAnchor.position : Vector3.zero;

    private IDialogueActorsRegistry _dialogueActorsRegistry;
    
    [Inject]
    public void Construct(IDialogueActorsRegistry registry)
    {
        _dialogueActorsRegistry  = registry;
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        
        _dialogueActorsRegistry.Register(this);
    }
    
    protected new void OnDisable()
    {
        base.OnDisable();
        
        _dialogueActorsRegistry.Unregister(this);
    }
}