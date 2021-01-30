using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoice
{
    public EventConditionGroup conditions;
    public string actionText;
    public List<EventOutcome> possibleOutcomes = new List<EventOutcome>();

    public bool ConditionsSatisfied(Stats stats)
    {
        return conditions.Evaluate(stats);
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
}
