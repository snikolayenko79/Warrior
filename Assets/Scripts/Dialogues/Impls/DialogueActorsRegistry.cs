using System.Collections.Generic;
using UnityEngine;

public class DialogueActorsRegistry : IDialogueActorsRegistry
{
    private readonly Dictionary<string, IDialogueActor> _actors = new();

    public void Register(IDialogueActor actor)
    {
        if (!_actors.TryAdd(actor.ActorId, actor))
        {
            Debug.LogError($"[Dialogue] Дубликат ID персонажа: {actor.ActorId}!");
        }
    }

    public void Unregister(IDialogueActor actor)
    {
        _actors.Remove(actor.ActorId);
    }

    public IDialogueActor GetActor(string actorId)
    {
        if (_actors.TryGetValue(actorId, out var actor))
        {
            return actor;
        }
        
        Debug.LogWarning($"[Dialogue] Персонаж с ID '{actorId}' не зарегистрирован.");
        return null;
    }
}