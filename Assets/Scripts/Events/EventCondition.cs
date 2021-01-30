using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
    List<List<EventCondition>> internalConditions = new List<List<EventCondition>>();

    public EventConditionGroup(string conditions)
    {
        ParseConditions(conditions);
    }

    public bool Evaluate(Stats stats)
    {
        if (internalConditions != null)
        {
            if (internalConditions.Count == 0)
            {
                return true;
            }
            // One of these must be true
            foreach (var andConditionsGroup in internalConditions)
            {
                if (EvaluateAndConditions(stats, andConditionsGroup))
                    return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    bool EvaluateAndConditions(Stats stats, List<EventCondition> conditionList)
    {
        // All conditions must be true
        if (conditionList != null)
        {
            foreach(var cond in conditionList)
            {
                if (!cond.Evaluate(stats))
                {
                    return false;
                }
            }
        }
        return true;
    }

    void ParseConditions(string conditions)
    {
        internalConditions.Clear();
        if (!string.IsNullOrEmpty(conditions))
        {
            var orGroups = conditions.Split('|');
            foreach (var group in orGroups)
            {
                var andList = new List<EventCondition>();
                var andGroups = group.Split('&');
                foreach (var andCondition in andGroups)
                {
                    var trimmed = andCondition.Trim();
                    andList.Add(new EventCondition(trimmed));
                }
                internalConditions.Add(andList);
            }
        }
    }

    public override string ToString()
    {
        if (internalConditions != null)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < internalConditions.Count; i++)
            {
                var andGroup = internalConditions[i];
                for (int y = 0; y < andGroup.Count; y++)
                {
                    sb.Append(andGroup[y].ToString());
                    if (y < andGroup.Count - 1)
                    {
                        sb.Append(" & ");
                    }
                }

                if (i < internalConditions.Count - 1)
                {
                    sb.Append(" | ");
                }
            }

            return sb.ToString();
        }
        else
        {
            return "";
        }
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
