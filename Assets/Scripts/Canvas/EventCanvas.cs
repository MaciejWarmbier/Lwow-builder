using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class EventCanvas : MonoBehaviour, ICanvas
{
    public Action OnClose;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button rightChoiceButton;
    [SerializeField] private Button leftChoiceButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI rightChoiceLabel;
    [SerializeField] private TextMeshProUGUI leftChoiceLabel;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI eventName;


    private List<string> descriptions = new List<string>();
    private int descriptionIndex = 0;
    private bool eventHasEnded = false;
    public bool EventHasEnded { get { return eventHasEnded; } }
    private GameEvent _event;
    private int pageNumber = 1;
    private int previousPage = 1;
    private ResourcesController _resourcesController;
    private GameEventsController _gameEventsController;
    private GridManager _gridController;
    private PlotController _plotController;

    private void Awake()
    {
        Assert.IsNotNull(nextButton);
        Assert.IsNotNull(previousButton);
        Assert.IsNotNull(rightChoiceButton);
        Assert.IsNotNull(leftChoiceButton);
        Assert.IsNotNull(continueButton);
        Assert.IsNotNull(rightChoiceLabel);
        Assert.IsNotNull(leftChoiceLabel);
        Assert.IsNotNull(description);
        Assert.IsNotNull(eventName);

        continueButton.gameObject.SetActive(false);
        _resourcesController = GameController.Game.GetController<ResourcesController>();
        _gameEventsController = GameController.Game.GetController<GameEventsController>();
        _gridController = GameController.Game.GetController<GridManager>();
        _plotController = GameController.Game.GetController<PlotController>();
    }

    public void CloseCanvas()
    {
        OnClose?.Invoke();
        eventHasEnded = true;
        GameController.Game.UnPauseGame();
        Destroy(gameObject);
    }

    public void ShowEvent(GameEvent eventData)
    {
        if (eventData != null)
        {
            _event = eventData;
            if (!eventData.skipChoice)
            {
                descriptions.Add(eventData.description.AddSpriteTextToStrings());
                if (!string.IsNullOrEmpty(eventData.description2))
                    descriptions.Add(eventData.description2.AddSpriteTextToStrings());
                if (!string.IsNullOrEmpty(eventData.description3))
                    descriptions.Add(eventData.description3.AddSpriteTextToStrings());
                if (!string.IsNullOrEmpty(eventData.description4))
                    descriptions.Add(eventData.description4.AddSpriteTextToStrings());

                if (eventData.type == EventType.TheGreatHunt)
                {
                    if(_plotController.hasSword)
                        descriptions.Add("... but we have the sword of Perun. This beast can’t stand the true gift from the God.");
                    if (_plotController.hasKupalaFlower)
                        descriptions.Add("Let’s not forget that we still have the Daryas’ Fern flower. It could help with the battle.");

                }

                eventHasEnded = false;
                description.text = eventData.description.AddSpriteTextToStrings();
                rightChoiceLabel.text = eventData.rightChoice.choiceText.AddSpriteTextToStrings();
                leftChoiceLabel.text = eventData.leftChoice.choiceText.AddSpriteTextToStrings();
                eventName.text = eventData.title;
                CheckButtonFunctionality();
            }
            else
            {
                SkipChoice();
            }
        }
    }
    
    private void SkipChoice()
    {
        descriptions.Add(_event.rightChoice.choiceResultText.AddSpriteTextToStrings());
        if (!string.IsNullOrEmpty(_event.rightChoice.choiceResultText2))
            descriptions.Add(_event.rightChoice.choiceResultText2.AddSpriteTextToStrings());
        if (!string.IsNullOrEmpty(_event.rightChoice.choiceResultText3))
            descriptions.Add(_event.rightChoice.choiceResultText3.AddSpriteTextToStrings());
        if (!string.IsNullOrEmpty(_event.rightChoice.choiceResultText4))
            descriptions.Add(_event.rightChoice.choiceResultText4.AddSpriteTextToStrings());

        _gameEventsController.RightEventResults(_event);
        eventHasEnded = false;
        description.text = descriptions[0].AddSpriteTextToStrings();
        eventName.text = _event.title;
        CheckButtonFunctionality();
        ShowContinueButton();
    }

    public void OnChoiceClick(bool isRight)
    {
        pageNumber = 1;
        CheckButtonFunctionality();

        Choice selectedChoice;
        if (isRight)
        {
            selectedChoice = _event.rightChoice;
            _gameEventsController.RightEventResults(_event);
        }
        else
        {
            selectedChoice = _event.leftChoice;
            _gameEventsController.WrongEventResults(_event);
        }


        descriptions.Clear();
        descriptions.Add(selectedChoice.choiceResultText.AddSpriteTextToStrings());
        if(!string.IsNullOrEmpty(selectedChoice.choiceResultText2))
            descriptions.Add(selectedChoice.choiceResultText2.AddSpriteTextToStrings());
        if (!string.IsNullOrEmpty(selectedChoice.choiceResultText3))
            descriptions.Add(selectedChoice.choiceResultText3.AddSpriteTextToStrings());
        if (!string.IsNullOrEmpty(selectedChoice.choiceResultText4))
            descriptions.Add(selectedChoice.choiceResultText4.AddSpriteTextToStrings());

        if(_event.type == EventType.TheGreatHunt && isRight)
        {
            EndGameDialogue();
        }

        descriptionIndex = 0;

        _resourcesController.ChangeFood(selectedChoice.foodChange);
        _resourcesController.ChangeMorale(selectedChoice.moraleChange);
        _resourcesController.ChangeResources(selectedChoice.resourcesChange);
        description.text = selectedChoice.choiceResultText.AddSpriteTextToStrings();
        CheckButtonFunctionality();
        ShowContinueButton();
    }

    private void EndGameDialogue()
    {
        if (_plotController.hasSword)
            descriptions.Add("The sword of Perun sparked with the lightning power enchanted within the blade, and Daniel was confident that this day will be the last of this enormous Lion.");
        if (_plotController.hasKupalaFlower)
            descriptions.Add("The Fern flower disappeared as the soldiers felt unstoppable.");

            descriptions.Add("The fight was long and tiring. Few of the villagers fell, but Daniel did not stop attacking the beast. The lion was jumping all over the place, but it began to growl out of pain. Suddenly the Lion jumped on Daniel and tried to bite his head off when Daniel grabbed his fangs and was trying to wrestle the beast.");

        if (_plotController.hasSword)
        {
            descriptions.Add("He managed to toss the beast to the ground when one of the villagers swung a final blow to the beast. The Lion was dead.");
            descriptions.Add("People in the town watched when the villagers came down the hill with the head of a lion. The cheering and applauding rose and Daniel became a legend! That is how the town was saved and was named Leviv after that act of bravery.");
            descriptions.Add("Game Over. You killed the Lion. Thank you for playing <3");
        }
        else if (_plotController.hasKupalaFlower)
        {
            descriptions.Add("He managed to toss the beast to the ground as he reached for the blade of the God, and sink it into his throat. The Lion was dead.");
            descriptions.Add("People in the town watched when the villagers came down the hill with the head of a lion. The cheering and applauding rose and Daniel became a legend! That is how the town was saved and was named Leviv after that act of bravery. ");
            descriptions.Add("Game Over. You killed the Lion. Thank you for playing <3");
        }
        else
        {
            descriptions.Add("but… the beast was stronger… Daniel died in the battle. Darya was waiting for her husband to return, but he never did… and the beast was still alive.");
            descriptions.Add("Game Over. You died. Thank you for playing <3");
        }
    }

    private void ShowContinueButton()
    {
        leftChoiceButton.gameObject.SetActive(false); ;
        rightChoiceButton.gameObject.SetActive(false); ;
        continueButton.gameObject.SetActive(true); ;
    }

    public void GoToNextPage()
    {
        nextButton.interactable = true;
        int pageCount = description.textInfo.pageCount;
        if (pageNumber == pageCount)
        {
            if (descriptions.Count > descriptionIndex + 1)
            {
                previousPage = pageCount;
                descriptionIndex++;
                description.text = descriptions[descriptionIndex];
            }
            pageNumber = 1;
        }
        else pageNumber++;

        description.pageToDisplay = pageNumber;

        CheckButtonFunctionality();
    }

    public void GoToPreviousPage()
    {
        previousButton.interactable = true;
        if (pageNumber == 1)
        {
            if(descriptionIndex > 0)
            {
                descriptionIndex--;
                description.text = descriptions[descriptionIndex];
            }

            pageNumber = previousPage;
        }
        else pageNumber--;
        
        description.pageToDisplay = pageNumber;

        CheckButtonFunctionality();
    }

    private async void CheckButtonFunctionality()
    {
        await Task.Delay(100);
        if (descriptionIndex == 0 && pageNumber == 1)
        {
            previousButton.interactable = false;
        }
        else
        {
            previousButton.interactable = true;
        }


        var pageCount = description.textInfo.pageCount;
        if (descriptionIndex == descriptions.Count - 1 && pageCount == pageNumber)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
