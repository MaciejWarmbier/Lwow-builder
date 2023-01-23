using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private List<string> pages;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI title;

    private List<string> descriptions = new List<string>();
    private int descriptionIndex = 0;

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
        ShowTutorial();
    }

    public void CloseCanvas()
    {
        WorldController.worldController.UnPauseGame();
        Destroy(gameObject);
    }

    public void ShowTutorial() 
    {
        WorldController.worldController.PauseGame();
        foreach (var page in pages)
        {
            descriptions.Add(page.ToString());
        }

        description.text = pages[0].AddSpriteTextToStrings();
        title.text = "Tutorial";
        CheckButtonFunctionality();
        if(WorldController.worldController.lastTile != null)
            WorldController.worldController.lastTile.StopHover();
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
}
