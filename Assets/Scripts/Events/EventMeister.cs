using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMeister : MonoBehaviour
{
    static EventMeister instance;
    void Awake()
    {
        instance = this;
    }

    public static GameEvent GetRandomEvent(Stats playerStats)
    {
        return instance.GetRandomEventInternal(playerStats);
    }

    private GameEvent GetRandomEventInternal(Stats playerStats)
    {
        return GameEvent.GetTestEvent();
    }
}
