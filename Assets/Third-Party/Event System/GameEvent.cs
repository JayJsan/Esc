using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    // Raise event through different method signatures
    public void Raise(Component sender, object data) 
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaise(sender, data);
        }
    }

    // Manage Listeners
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener)) listeners.Remove(listener);
    }
}
