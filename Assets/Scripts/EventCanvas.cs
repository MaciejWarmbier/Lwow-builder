using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class EventCanvas : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button rightChoiceButton;
    [SerializeField] private Button leftChoiceButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Image eventImage;
    [SerializeField] private TextMeshProUGUI rightChoiceLabel;
    [SerializeField] private TextMeshProUGUI leftChoiceLabel;
    [SerializeField] private TextMeshProUGUI description;

    private bool hasEnded = false;
    public bool HasEnded { get { return hasEnded; } }
    private GameEvent _event;
    private int pageNumber = 1;
    private void Awake()
    {
        Assert.IsNotNull(nextButton);
        Assert.IsNotNull(previousButton);
        Assert.IsNotNull(rightChoiceButton);
        Assert.IsNotNull(leftChoiceButton);
        Assert.IsNotNull(continueButton);
        Assert.IsNotNull(eventImage);
        Assert.IsNotNull(rightChoiceLabel);
        Assert.IsNotNull(leftChoiceLabel);
        Assert.IsNotNull(description);

        continueButton.gameObject.SetActive(false);
    }

    public void CloseCanvas()
    {
        hasEnded = true;
        WorldController.worldController.UnPauseGame();
        Destroy(gameObject);
    }

    public void ShowEvent(GameEvent eventData)
    {
        if (eventData != null)
        {
            hasEnded = false;
            _event = eventData;
            description.text = eventData.description;
            eventImage.sprite = eventData.image;
            rightChoiceLabel.text = eventData.rightChoice.choiceText;
            leftChoiceLabel.text = eventData.leftChoice.choiceText;
            description.text = eventData.description;
        }
    }
    
    public void OnChoiceClick(bool isRight)
    {
        Choice selectedChoice;
        if (isRight)
        {
            selectedChoice = _event.rightChoice;
        }
        else
        {
            selectedChoice = _event.leftChoice;
        }
        
        VillageResources.villageResources.ChangeFood(selectedChoice.foodChange);
        VillageResources.villageResources.ChangeMorale(selectedChoice.moraleChange);
        VillageResources.villageResources.ChangeResources(selectedChoice.resourcesChange);
        description.text = selectedChoice.choiceResultText;
        ShowContinueButton();
    }

    private void ShowContinueButton()
    {
        leftChoiceButton.gameObject.SetActive(false); ;
        rightChoiceButton.gameObject.SetActive(false); ;
        continueButton.gameObject.SetActive(true); ;
    }

    public void GoToNextPage()
    {
        int pageCount = description.textInfo.pageCount;
        if (pageNumber == pageCount) pageNumber = 1;
        else pageNumber++;
       
        description.pageToDisplay = pageNumber;
    }

    public void GoToPreviousPage()
    {
        if (pageNumber == 0) pageNumber = description.textInfo.pageCount;
        else pageNumber--;
        
        description.pageToDisplay = pageNumber;
    }
}
