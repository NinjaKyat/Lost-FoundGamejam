using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JsonFunTests
{
    const string testStat = "testStat";
    // A Test behaves as an ordinary method
    [Test]
    public void CanSerializeEventConditionGroup()
    {
        EventConditionGroup group = new EventConditionGroup();
        var andConditions = new AndConditions();
        andConditions.Conditions.Add(new EventCondition("hello = 1"));
        group.anyMustBeTrue.Add(andConditions);
        var json = JsonUtility.ToJson(group, true);
        Debug.Log(json);
        Assert.IsTrue(json.Length > 0);
    }

    [Test]
    public void CheckEventSerialization()
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

        var json = JsonUtility.ToJson(evt, true);
        Debug.Log(json);
    }
}
