using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ComparisonOperator
{
    Equals,
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual
}

[System.Serializable]
public class EventConditionGroup
{
    public List<AndConditions> anyMustBeTrue = new List<AndConditions>();

    public bool Evaluate(Stats stats)
    {
        foreach(var condition in anyMustBeTrue)
        {
            if (condition.Evaluate(stats))
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class AndConditions : ISerializationCallbackReceiver
{
    public List<EventCondition> Conditions { get; } = new List<EventCondition>();

    [SerializeField]
    List<string> allMustBeTrue = new List<string>();

    public void OnAfterDeserialize()
    {
        Conditions.Clear();

        foreach (var stringCondition in allMustBeTrue)
        {
            Conditions.Add(new EventCondition(stringCondition));
        }
    }

    public void OnBeforeSerialize()
    {
        allMustBeTrue.Clear();

        foreach (var cond in Conditions)
        {
            allMustBeTrue.Add(cond.ToString());
        }
    }

    public bool Evaluate(Stats stats)
    {
        foreach(var condition in Conditions)
        {
            if (!condition.Evaluate(stats))
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class EventCondition
{
    string targetStat = null;
    ComparisonOperator cmpOperator = ComparisonOperator.Equals;
    string statToCompare = null;
    int valueToCompare = 0;

    public EventCondition(string condition)
    {
        Initialize(condition);
    }

    void Initialize(string condition)
    {
        var cmpIndex = ParseComparisonOperator(condition);
        targetStat = condition.Substring(0, cmpIndex - 1);
        ParseValueToCompare(condition.Substring(cmpIndex + GetOffsetFromComparison()));
    }

    int ParseComparisonOperator(string condition)
    {
        var comparisonIndex = condition.IndexOf('>');
        if (comparisonIndex != -1)
        {
            if (condition[comparisonIndex + 1] == '=')
            {
                cmpOperator = ComparisonOperator.GreaterOrEqual;
            }
            else
            {
                cmpOperator = ComparisonOperator.Greater;
            }
        }
        else
        {
            comparisonIndex = condition.IndexOf('<');
            if (comparisonIndex != -1)
            {
                if (condition[comparisonIndex + 1] == '=')
                {
                    cmpOperator = ComparisonOperator.LessOrEqual;
                }
                else
                {
                    cmpOperator = ComparisonOperator.Less;
                }
            }
            else
            {
                comparisonIndex = condition.IndexOf('=');
                if (comparisonIndex != -1)
                {
                    cmpOperator = ComparisonOperator.Equals;
                }
                else
                {
                    throw new System.Exception("Couldn't parse comparison operator in condition: " + condition);
                }
            }
        }
        return comparisonIndex;
    }

    int GetOffsetFromComparison()
    {
        if (cmpOperator == ComparisonOperator.GreaterOrEqual || cmpOperator == ComparisonOperator.LessOrEqual)
            return 3;
        return 2;
    }

    void ParseValueToCompare(string substring)
    {
        int result = 0;
        if (int.TryParse(substring, out result))
        {
            valueToCompare = result;
        }
        else
        {
            statToCompare = substring;
        }
    }

    public bool Evaluate(Stats stats)
    {
        var firstValue = stats.GetStat(targetStat);
        var secondValue = string.IsNullOrEmpty(statToCompare) ? valueToCompare : stats.GetStat(statToCompare);
        switch (cmpOperator)
        {
            case ComparisonOperator.Equals:
                return firstValue == secondValue;
            case ComparisonOperator.Greater:
                return firstValue > secondValue;
            case ComparisonOperator.Less:
                return firstValue < secondValue;
            case ComparisonOperator.GreaterOrEqual:
                return firstValue >= secondValue;
            case ComparisonOperator.LessOrEqual:
                return firstValue <= secondValue;
        }
        throw new System.Exception("Failed to evaluate condition!");
    }

    int EvaluateStat(Stats stats, string target)
    {
        return stats.GetStat(target);
    }

    public override string ToString()
    {
        string comparisonString = GetComparisonOperatorString();

        return string.Format("{0} {1} {2}",
            targetStat, comparisonString, statToCompare ?? valueToCompare.ToString());
    }

    string GetComparisonOperatorString()
    {
        switch (cmpOperator)
        {
            case ComparisonOperator.Equals:
                return "=";
            case ComparisonOperator.Greater:
                return ">";
            case ComparisonOperator.Less:
                return "<";
            case ComparisonOperator.GreaterOrEqual:
                return ">=";
            case ComparisonOperator.LessOrEqual:
                return "<=";
        }
        return "=";
    }
}
