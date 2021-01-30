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
        var group = new EventConditionGroup("health > 0 & speed > 1");
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

    [Test]
    public void SerializeEventCollection()
    {
        var evt = GameEvent.GetTestEvent();
        var collection = new EventCollection();
        collection.events.Add(evt);
        collection.events.Add(evt);
        var json = JsonUtility.ToJson(collection, true);
        Debug.Log(json);
    }
}
