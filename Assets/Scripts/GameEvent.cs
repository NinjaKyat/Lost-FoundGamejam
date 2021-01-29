using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEvent
{
    
}

enum ComparisonOperator
{
    Equals,
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual
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
            return 2;
        return 1;
    }

    void ParseValueToCompare(string substring)
    {
        int result = 0;
        if (int.TryParse(substring, out result))
        {
            return;
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
        switch(cmpOperator)
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
}
