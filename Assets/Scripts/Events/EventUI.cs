using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    [SerializeField]
    Button buttonPrefab;
    [SerializeField]
    Text nameField;
    [SerializeField]
    Text descriptionField;
    [SerializeField]
    Transform choiceLocation;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Image eventImage;
    [SerializeField]
    Player player;

    public static EventUI instance;

    List<Button> currentButtons = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //yield return new WaitForSeconds(2f);
        //var evt = EventMeister.GetRandomEvent(player.playerStats);
        //DisplayEvent(evt);
    }

    public bool IsShowingUI()
    {
        return canvasGroup.alpha > 0f;
    }

    void ResetState()
    {
        foreach(var button in currentButtons)
        {
            Destroy(button.gameObject);
        }
        currentButtons.Clear();
    }

    void SetUIVisibility(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    public void DisplayEvent(GameEvent targetEvent)
    {
        ResetState();
        SetUIVisibility(true);
        nameField.text = targetEvent.name;
        descriptionField.text = targetEvent.description;
        eventImage.sprite = targetEvent.Image;
        foreach(var choice in targetEvent.choices)
        {
            if (choice.ConditionsSatisfied(player.playerStats))
            {
                CreateButtonForChoice(targetEvent, choice);
            }
        }
    }

    public void DisplayOutcome(GameEvent mainEvent, EventOutcome outcome)
    {
        ResetState();
        SetUIVisibility(true);
        nameField.text = mainEvent.name;
        descriptionField.text = outcome.outcomeText + "\n\n" + ExtractStatChangesFromOutcome(outcome);
        eventImage.sprite = mainEvent.Image;
        CreateButton("Okay", () =>
        {
            SetUIVisibility(false);
            outcome.Evaluate(player.playerStats);
        });
    }

    string ExtractStatChangesFromOutcome(EventOutcome outcome)
    {
        var sb = new System.Text.StringBuilder();
        foreach(var action in outcome.Actions)
        {
            var operatorString = "=";
            if (action.stateOperator == StateOperator.Add)
            {
                operatorString = "+";
            }
            else if (action.stateOperator == StateOperator.Subtract)
            {
                operatorString = "-";
            }

            sb.AppendFormat("{0} {1}{2}\n", action.targetStat, operatorString, action.statToUse ?? action.numberToUse.ToString());
        }
        return sb.ToString();
    }


    public void DisplayDialog(string name, string description, string icon, System.Action onClick)
    {
        ResetState();
        SetUIVisibility(true);
        nameField.text = name;
        eventImage.sprite = EventMeister.GetImage(icon);
        descriptionField.text = description;
        CreateButton("Okay", () => {
            SetUIVisibility(false);
            onClick?.Invoke();
        });
    }



    public void CreateButtonForChoice(GameEvent mainEvent, EventChoice choice)
    {
        CreateButton(choice.actionText, () => { PerformChoice(mainEvent, choice); });
    }

    public void CreateButton(string text, System.Action onClick)
    {
        var buttonInstance = Instantiate(buttonPrefab);
        buttonInstance.transform.SetParent(choiceLocation);
        buttonInstance.GetComponentInChildren<Text>().text = text;
        buttonInstance.onClick.AddListener(() =>
        {
            onClick();
        });
        currentButtons.Add(buttonInstance);
    }

    public void PerformChoice(GameEvent mainEvent, EventChoice choice)
    {
        var outcome = choice.PerformChoice();
        DisplayOutcome(mainEvent, outcome);
    }
}
