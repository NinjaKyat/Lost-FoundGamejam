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
    Player player;

    static EventUI instance;

    List<Button> currentButtons = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    void ResetState()
    {
        foreach(var button in currentButtons)
        {
            Destroy(button);
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
        foreach(var choice in targetEvent.choices)
        {
            if (choice.ConditionsSatisfied(player.playerStats))
            {
                CreateButtonForChoice(choice);
            }
        }
    }

    public void DisplayOutcome(EventOutcome outcome)
    {
        ResetState();
        SetUIVisibility(true);
        descriptionField.text = outcome.outcomeText;
        CreateButton("Okay", () =>
        {
            SetUIVisibility(false);
            outcome.Evaluate(player.playerStats);
        });
    }

    public void CreateButtonForChoice(EventChoice choice)
    {
        CreateButton(choice.actionText, () => { PerformChoice(choice); });
    }

    public void CreateButton(string text, System.Action onClick)
    {
        var buttonInstance = Instantiate(buttonPrefab);
        buttonInstance.transform.SetParent(choiceLocation);
        buttonInstance.GetComponent<Text>().text = text;
        buttonInstance.onClick.AddListener(() =>
        {
            onClick();
        });
    }

    public void PerformChoice(EventChoice choice)
    {
        var outcome = choice.PerformChoice();
    }
}
