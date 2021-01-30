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
        var evt = GameEvent.GetTestEvent();

        var json = JsonUtility.ToJson(evt, true);
        Debug.Log(json);
    }
}
