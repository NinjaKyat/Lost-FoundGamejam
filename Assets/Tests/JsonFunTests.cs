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
        eatChoice.actionText = "Eat the berries.";
        eatChoice.outcomeText = "The berries taste sweet, but you get some nasty stomach cramps.";
        eatChoice.OutcomeAction.Add(new EventAction("health -= 1"));
        eatChoice.OutcomeAction.Add(new EventAction("food += 1"));
        evt.choices.Add(eatChoice);

        var leaveChoice = new EventChoice();
        leaveChoice.actionText = "Leave the berries.";
        leaveChoice.outcomeText = "You don't know if these are safe to eat, better leave them.";
        evt.choices.Add(leaveChoice);

        var json = JsonUtility.ToJson(evt, true);
        Debug.Log(json);
    }
}
