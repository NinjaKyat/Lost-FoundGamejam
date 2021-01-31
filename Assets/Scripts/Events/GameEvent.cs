using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEvent : ISerializationCallbackReceiver
{
    public string name;
    public string description;
    public string tags;
    public EventConditionGroup Conditions { get; set; }
    [SerializeField]
    string conditions;

    Sprite imageInternal;
    public Sprite Image { get
        {
            if (imageInternal == null)
            {
                return EventMeister.GetImage("");
            }
            else
            {
                return imageInternal;
            }
        }
    }
    [SerializeField]
    string image;

    public List<EventChoice> choices = new List<EventChoice>();

    public static GameEvent GetTestEvent()
    {
        GameEvent evt = new GameEvent();
        evt.description = "You find berries.";
        evt.name = "Food?";

        var eatChoice = new EventChoice();
        evt.choices.Add(eatChoice);
        eatChoice.actionText = "Eat the berries.";
        var poisonedOutcome = new EventOutcome();
        eatChoice.possibleOutcomes.Add(poisonedOutcome);
        poisonedOutcome.chance = 50;
        poisonedOutcome.outcomeText = "Suddenly you don't feel so well. The berries were poisonous!";
        poisonedOutcome.Actions.Add(new EventAction("health -= 1"));
        poisonedOutcome.Actions.Add(new EventAction("food -= 1"));
        poisonedOutcome.Actions.Add(new EventAction("berryKnowledge += 1"));
        var tastyOutcome = new EventOutcome();
        eatChoice.possibleOutcomes.Add(tastyOutcome);
        tastyOutcome.outcomeText = "The berries are delicious! You feel a bit less hungry.";
        tastyOutcome.Actions.Add(new EventAction("food += 1"));
        tastyOutcome.Actions.Add(new EventAction("berryKnowledge += 1"));

        var leaveChoice = new EventChoice();
        evt.choices.Add(leaveChoice);
        leaveChoice.actionText = "Leave the berries.";
        var leaveOutcome = new EventOutcome();
        leaveChoice.possibleOutcomes.Add(leaveOutcome);
        leaveOutcome.outcomeText = "You don't know if these are safe to eat, better leave them.";

        var knowledgeChoice = new EventChoice();
        evt.choices.Add(knowledgeChoice);
        knowledgeChoice.actionText = "Use your knowledge to eat only the edible berries.";
        knowledgeChoice.Conditions = new EventConditionGroup("berryKnowledge >= 3 | encyclopedia > 0 & canRead > 0");
        var deliciousOutcome = new EventOutcome();
        deliciousOutcome.outcomeText = "You recognize the edible berries and avoid the poisonous ones. You feel less hungry.";
        deliciousOutcome.Actions.Add(new EventAction("food += 1"));
        knowledgeChoice.possibleOutcomes.Add(deliciousOutcome);
        return evt;
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
        imageInternal = EventMeister.GetImage(image);
    }
}

[System.Serializable]
public class EventCollection
{
    public List<GameEvent> events = new List<GameEvent>();
}


