using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCanvas : MonoBehaviour
{
    [SerializeField] GameObject descriptionCanvas;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Image icon;

    [SerializeField] GameObject constructionCanvas;
    [SerializeField] Image constructionImage;
    [SerializeField] List<Sprite> constructionSprites;

    private Building _building;
    public void Setup(Building building)
    {
        _building = building;
    }

    public void ShowBuildingDescription()
    {
        nameText.text = _building.Data.Name;
        descriptionText.text = _building.Description().AddSpriteTextToStrings();
        icon.sprite = _building.Data.BuildingImage;

        descriptionCanvas.SetActive(true);
    }

    public void HideBuildingDescription()
    {
        descriptionCanvas.SetActive(false);
    }

    public void UpdateConstructionTimer(int index)
    {
        if(!constructionCanvas.activeSelf) constructionCanvas.SetActive(true);

        constructionImage.sprite = constructionSprites[index];
    }

    public void HideConstructionTimer()
    {
        constructionCanvas.SetActive(false);
    }
}
