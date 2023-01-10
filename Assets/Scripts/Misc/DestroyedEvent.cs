using System;
using UnityEngine;

/// <summary>
/// Will be implamented at a later time
/// </summary>

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool playerDied, int points)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs() { playerDied = playerDied, points = points });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool playerDied;
    public int points;
}
