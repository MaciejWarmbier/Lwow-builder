using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TutorialCanvas : MonoBehaviour, ICanvas
{
    public Action OnClose;

    [SerializeField] private List<string> pages;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI title;

    private List<string> descriptions = new List<string>();
    private int descriptionIndex = 0;
    private GridManager _gridController;

    private int pageNumber = 1;
    private int previousPage = 1;

    private void Awake()
    {
        Assert.IsNotNull(nextButton);
        Assert.IsNotNull(previousButton);
        Assert.IsNotNull(startButton);
        Assert.IsNotNull(description);
        Assert.IsNotNull(title);
    }

    private void Start()
    {
        _gridController = GameController.Game.GetController<GridManager>();
    }

    public void CloseCanvas()
    {
        GameController.Game.UnPauseGame();
        OnClose?.Invoke();
        SetActive(false);
    }

    public void ShowTutorial() 
    {
        GameController.Game.PauseGame();
        foreach (var page in pages)
        {
            descriptions.Add(page.ToString());
        }

        //description.text = pages[0].AddSpriteTextToStrings();
        description.SetText(pages[0].AddSpriteTextToStrings().Replace("<enter>", "\n"));
        title.text = "Tutorial";
        CheckButtonFunctionality();
        if(_gridController?.lastChosenTile != null)
            _gridController.lastChosenTile.StopHover();

        description.enabled = false;
        description.enabled = true;
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
                description.SetText(descriptions[descriptionIndex].AddSpriteTextToStrings().Replace("<enter>", "\n"));
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
                description.SetText(descriptions[descriptionIndex].AddSpriteTextToStrings().Replace("<enter>", "\n"));
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
