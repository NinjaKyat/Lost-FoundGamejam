using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoice : ISerializationCallbackReceiver
{
    public EventConditionGroup conditions;
    public string actionText;
    public string outcomeText;
    public List<EventAction> OutcomeAction { get; } = new List<EventAction>();
    [SerializeField]
    List<string> actions = new List<string>();

    public void OnBeforeSerialize()
    {
        actions.Clear();
        foreach(var action in OutcomeAction)
        {
            actions.Add(action.ToString());
        }
    }

    public void OnAfterDeserialize()
    {
        OutcomeAction.Clear();
        foreach(var action in actions)
        {
            OutcomeAction.Add(new EventAction(action));
        }
    }
}
