using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EventAction
{
    public StateOperator stateOperator;
    public string targetStat;
    public string statToUse = null;
    public int numberToUse = 0;

    public EventAction(string action)
    {
        var equalityIndex = ParseEqualityOperator(action);
        targetStat = action.Substring(0, equalityIndex - 1);
        ParseValueToUse(action.Substring(equalityIndex + GetOperatorOffset()));
    }

    int ParseEqualityOperator(string action)
    {
        var index = action.IndexOf('=');
        if (index != -1)
        {
            var previousCharacter = action[index - 1];
            if (previousCharacter == '+')
            {
                stateOperator = StateOperator.Add;
                index--;
            }
            else if (previousCharacter == '-')
            {
                stateOperator = StateOperator.Subtract;
                index--;
            }
            else
            {
                stateOperator = StateOperator.Set;
            }
        }
        else
        {
            throw new Exception("Failed to parse event action " + action);
        }

        return index;
    }

    int GetOperatorOffset()
    {
        if (stateOperator == StateOperator.Set)
            return 2;
        return 3;
    }

    void ParseValueToUse(string value)
    {
        var intValue = 0;
        if (int.TryParse(value, out intValue))
        {
            numberToUse = intValue;
        }
        else
        {
            statToUse = value;
        }
    }

    public override string ToString()
    {
        return string.Format("{0} {1} {2}", targetStat, GetOperatorString(), statToUse ?? numberToUse.ToString());
    }

    public void Evaluate(Stats stats)
    {
        var valueToUse = string.IsNullOrEmpty(statToUse) ? numberToUse : stats.GetStat(statToUse);
        switch(stateOperator)
        {
            case StateOperator.Set:
                stats.SetStat(targetStat, valueToUse);
                break;
            case StateOperator.Add:
                stats.AddStat(targetStat, valueToUse);
                break;
            case StateOperator.Subtract:
                stats.AddStat(targetStat, -valueToUse);
                break;
        }
    }

    string GetOperatorString()
    {
        switch(stateOperator)
        {
            case StateOperator.Add:
                return "+=";
            case StateOperator.Set:
                return "=";
            case StateOperator.Subtract:
                return "-=";
        }
        return "=";
    }
}

public enum StateOperator
{
    Set,
    Add,
    Subtract
}