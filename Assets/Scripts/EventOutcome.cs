using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EventOutcome : ISerializationCallbackReceiver
{
    public int chance = 0;
    public string outcomeText;
    public List<EventAction> Actions { get; } = new List<EventAction>();
    [SerializeField]
    List<string> actions = new List<string>();


    public void OnBeforeSerialize()
    {
        actions.Clear();
        foreach (var action in Actions)
        {
            actions.Add(action.ToString());
        }
    }

    public void OnAfterDeserialize()
    {
        Actions.Clear();
        foreach (var action in actions)
        {
            Actions.Add(new EventAction(action));
        }
    }
}
