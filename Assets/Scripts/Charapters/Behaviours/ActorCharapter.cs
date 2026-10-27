using System.Reflection.Emit;
using UnityEngine;
using Zenject;

public class ActorCharapter : Character, IDialogueActor
{
    public string dialogueActorName = "";
    public Transform speechBubbleAnchor;
    
    public string ActorId => dialogueActorName;
    public string DisplayName => dialogueActorName;
    public Vector3 SpeechBubbleAnchor => speechBubbleAnchor != null? speechBubbleAnchor.position : Vector3.zero;

    private IDialogueActorsRegistry _registry;
    
    [Inject]
    public void Construct(IDialogueActorsRegistry registry)
    {
        _registry  = registry;
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        
        _registry.Register(this);
    }
    
    protected new void OnDisable()
    {
        base.OnDisable();
        
        _registry.Unregister(this);
    }
}