using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEvent
{
    public string name;
    public string description;
    public EventConditionGroup conditions;
    public List<EventChoice> choices = new List<EventChoice>();
}


