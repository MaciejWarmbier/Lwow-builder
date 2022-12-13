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
    [SerializeField] private TextMeshProUGUI rightChoiceLabel;
    [SerializeField] private TextMeshProUGUI leftChoiceLabel;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI eventName;


    private List<string> descriptions = new List<string>();
    private int descriptionIndex = 0;
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

    public string ChangeStrings(string text)
    {
        string newText = text.Replace("{f}", "<sprite=1>");
        newText = newText.Replace("{m}", "<sprite=2>");
        newText = newText.Replace("{r}", "<sprite=0>");

        return newText;
    }

    public void ShowEvent(GameEvent eventData)
    {
        if (eventData != null)
        {
            _event = eventData;
            if (!eventData.skipChoice)
            {
                descriptions.Add(ChangeStrings(eventData.description));
                if (!string.IsNullOrEmpty(eventData.description2))
                    descriptions.Add(ChangeStrings(eventData.description2));
                if (!string.IsNullOrEmpty(ChangeStrings(eventData.description3)))
                    descriptions.Add(ChangeStrings(eventData.description3));
                if (!string.IsNullOrEmpty(eventData.description4))
                    descriptions.Add(ChangeStrings(eventData.description4));

                if (eventData.isGreatHunt)
                {
                    if(WorldController.worldController.hasSword)
                        descriptions.Add("... but we have the sword of Perun. This beast can’t stand the true gift from the God.");
                    if (WorldController.worldController.hasKupalaFlower)
                        descriptions.Add("Let’s not forget that we still have the Daryas’ Fern flower. It could help with the battle.");

                }

                if (eventData.isPerunEvent)
                {
                    WorldController.worldController.isPerunActivated = true;
                }

                hasEnded = false;
                description.text = ChangeStrings(eventData.description);
                rightChoiceLabel.text = ChangeStrings(eventData.rightChoice.choiceText);
                leftChoiceLabel.text = ChangeStrings(eventData.leftChoice.choiceText);
                eventName.text = eventData.title;
            }
            else
            {
                SkipChoice();
            }
        }
    }
    
    private void SkipChoice()
    {
        descriptions.Add(ChangeStrings(_event.rightChoice.choiceResultText));
        if (!string.IsNullOrEmpty(_event.rightChoice.choiceResultText2))
            descriptions.Add(ChangeStrings(_event.rightChoice.choiceResultText2));
        if (!string.IsNullOrEmpty(_event.rightChoice.choiceResultText4))
            descriptions.Add(ChangeStrings(_event.rightChoice.choiceResultText3));
        if (!string.IsNullOrEmpty(_event.rightChoice.choiceResultText3))
            descriptions.Add(ChangeStrings(_event.rightChoice.choiceResultText4));

        hasEnded = false;
        description.text = ChangeStrings(descriptions[0]);
        eventName.text = _event.title;
        ShowContinueButton();
    }

    public void OnChoiceClick(bool isRight)
    {
        Choice selectedChoice;
        if (isRight)
        {
            selectedChoice = _event.rightChoice;
            RightChoiceResults();
        }
        else
        {
            selectedChoice = _event.leftChoice;
        }

        descriptions.Clear();
        descriptions.Add(ChangeStrings(selectedChoice.choiceResultText));
        if(!string.IsNullOrEmpty(selectedChoice.choiceResultText2))
            descriptions.Add(ChangeStrings(selectedChoice.choiceResultText2));
        if (!string.IsNullOrEmpty(selectedChoice.choiceResultText3))
            descriptions.Add(ChangeStrings(selectedChoice.choiceResultText3));
        if (!string.IsNullOrEmpty(selectedChoice.choiceResultText4))
            descriptions.Add(ChangeStrings(selectedChoice.choiceResultText4));

        if (_event.isGreatHunt && WorldController.worldController.hasSword && isRight)
            descriptions.Add("The sword of Perun sparked with the lightning power enchanted within the blade, and Daniel was confident that this day will be the last of this enormous Lion.");
        if (_event.isGreatHunt && WorldController.worldController.hasKupalaFlower && isRight)
            descriptions.Add("The Fern flower disappeared as the soldiers felt unstoppable.");
        if(_event.isGreatHunt && isRight)
            descriptions.Add("The fight was long and tiring. Few of the villagers fell, but Daniel did not stop attacking the beast. The lion was jumping all over the place, but it began to growl out of pain. Suddenly the Lion jumped on Daniel and tried to bite his head off when Daniel grabbed his fangs and was trying to wrestle the beast.");
        if (_event.isGreatHunt && WorldController.worldController.hasSword && isRight)
        {
            descriptions.Add("He managed to toss the beast to the ground when one of the villagers swung a final blow to the beast. The Lion was dead.");
            descriptions.Add("People in the town watched when the villagers came down the hill with the head of a lion. The cheering and applauding rose and Daniel became a legend! That is how the town was saved and was named Leviv after that act of bravery.");
            descriptions.Add("Game Over. You killed the Lion. Thank you for playing <3");
        }
        else if (_event.isGreatHunt && WorldController.worldController.hasKupalaFlower && isRight)
        {
            descriptions.Add("He managed to toss the beast to the ground as he reached for the blade of the God, and sink it into his throat. The Lion was dead.");
            descriptions.Add("People in the town watched when the villagers came down the hill with the head of a lion. The cheering and applauding rose and Daniel became a legend! That is how the town was saved and was named Leviv after that act of bravery. ");
            descriptions.Add("Game Over. You killed the Lion. Thank you for playing <3");
        }
        else if(_event.isGreatHunt && isRight)
        {
            descriptions.Add("but… the beast was stronger… Daniel died in the battle. Darya was waiting for her husband to return, but he never did… and the beast was still alive.");
            descriptions.Add("Game Over. You died. Thank you for playing <3");
        }

        descriptionIndex = 0;

        VillageResources.villageResources.ChangeFood(selectedChoice.foodChange);
        VillageResources.villageResources.ChangeMorale(selectedChoice.moraleChange);
        VillageResources.villageResources.ChangeResources(selectedChoice.resourcesChange);
        description.text = ChangeStrings(selectedChoice.choiceResultText);
        ShowContinueButton();
    }

    public void RightChoiceResults()
    {
        if (_event.isNocKupaly)
        {
            WorldController.worldController.hasKupalaFlower = true;
        }else if (_event.isSlayerOfTheBeast)
        {
            WorldController.worldController.isArmoryUnlocked = true;
        }
        else if (_event.isNymphEvent)
        {
            WorldController.worldController.isWheatBetter = true;
            WorldController.worldController.IncreaseWheat();
        }
        else if (_event.isPerunEvent)
        {
            WorldController.worldController.isPerunHappy = true;
            WorldController.worldController.BeginStorm();
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
        int pageCount = description.textInfo.pageCount;
        if (pageNumber == pageCount)
        {
            if (descriptions.Count > descriptionIndex + 1)
            {
                descriptionIndex++;
                description.text = descriptions[descriptionIndex];
            }
            else if(descriptionIndex == descriptions.Count - 1)
            {
                descriptionIndex = 0;
                description.text = descriptions[descriptionIndex];
            }
            pageNumber = 1;
        }
        else pageNumber++;
       
        description.pageToDisplay = pageNumber;
    }

    public void GoToPreviousPage()
    {
        //TODO
        if (pageNumber == 0) pageNumber = description.textInfo.pageCount;
        else pageNumber--;
        
        description.pageToDisplay = pageNumber;
    }
}
