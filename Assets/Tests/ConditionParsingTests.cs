using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ConditionParsingTests
{
    const string testStat = "testStat";
    // A Test behaves as an ordinary method
    [Test]
    public void TestingStatGreaterThanFivePasses()
    {
        var stats = new Stats();
        stats.AddStat(testStat, 10);
        var condition = new EventCondition($"{testStat} > 5");
        Assert.IsTrue(condition.Evaluate(stats));
    }

    [Test]
    public void TestingStatEqualToThreePasses()
    {
        var stats = new Stats();
        stats.AddStat(testStat, 3);
        var condition = new EventCondition($"{testStat} = 3");
        Assert.IsTrue(condition.Evaluate(stats));
    }

    [Test]
    public void TestingStatIsLessThanOrEqualsPasses()
    {
        var stats = new Stats();
        stats.AddStat(testStat, 1);
        var condition = new EventCondition($"{testStat} <= 3");
        Assert.IsTrue(condition.Evaluate(stats));
    }

    [Test]
    public void TestingTwoStatComparisonLessThanOrEqualsWorks()
    {
        var stats = new Stats();
        const string otherStat = "otherStat";
        stats.AddStat(testStat, 2);
        stats.AddStat(otherStat, 3);
        var condition = new EventCondition($"{testStat} <= {otherStat}");
        Assert.IsTrue(condition.Evaluate(stats));
    }

    [Test]
    public void ConditionGroupWithAndCorrectlyInterpreted()
    {
        var stats = new Stats();
        stats.SetStat("health", 7);
        var evt = new EventConditionGroup("health > 5 & health < 10");
        Assert.IsTrue(evt.Evaluate(stats));
    }

    [Test]
    public void ConditionGroupWithAndCorrectlyFalse()
    {
        var stats = new Stats();
        stats.SetStat("health", 12);
        var evt = new EventConditionGroup("health > 5 & health < 10");
        Assert.IsFalse(evt.Evaluate(stats));
    }

    [Test]
    public void ConditionGroupWithOrStatementCorrectlyTrue()
    {
        var stats = new Stats();
        stats.SetStat("health", 5);
        var evt = new EventConditionGroup("health > 10 | health < 7");
        Assert.IsTrue(evt.Evaluate(stats));
    }

    [Test]
    public void ConditionGroupWithOrAndAndCorrectlyFalse()
    {
        var stats = new Stats();
        stats.SetStat("health", 1);
        stats.SetStat("speed", 1);
        var evt = new EventConditionGroup("health > 5 | health > 0 & speed > 5");
        Assert.IsFalse(evt.Evaluate(stats));
    }
}
