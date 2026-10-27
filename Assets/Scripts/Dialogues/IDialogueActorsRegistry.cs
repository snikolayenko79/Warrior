using UnityEngine;

public interface IDialogueActorsRegistry
{
    void Register(IDialogueActor actor);
    void Unregister(IDialogueActor actor);
    IDialogueActor GetActor(string actorId);
}
