using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoice
{
    public EventConditionGroup conditions;
    public string actionText;
    public List<EventOutcome> possibleOutcomes = new List<EventOutcome>();
}
