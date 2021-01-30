using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoice : ISerializationCallbackReceiver
{
    public EventConditionGroup Conditions { get; set; }
    [SerializeField]
    string conditions;
    public string actionText;
    public List<EventOutcome> possibleOutcomes = new List<EventOutcome>();

    public bool ConditionsSatisfied(Stats stats)
    {
        if (Conditions != null)
        {
            return Conditions.Evaluate(stats);
        }
        return true;
    }

    public EventOutcome PerformChoice()
    {
        foreach(var outcome in possibleOutcomes)
        {
            if (outcome.chance > 0)
            {
                if (Random.Range(0, 100) < outcome.chance)
                {
                    return outcome;
                }
            }
            else
            {
                return outcome;
            }
        }

        return possibleOutcomes[0];
    }

    public void OnBeforeSerialize()
    {
        if (Conditions != null)
        {
            conditions = Conditions.ToString();
        }
        else
        {
            conditions = "";
        }
    }

    public void OnAfterDeserialize()
    {
        Conditions = new EventConditionGroup(conditions);
    }
}
