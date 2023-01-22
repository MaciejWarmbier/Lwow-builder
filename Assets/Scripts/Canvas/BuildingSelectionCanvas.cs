using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BuildingSelectionCanvas : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI resourcesPriceLabel;
    [SerializeField] private TextMeshProUGUI foodPriceLabel;
    [SerializeField] private TextMeshProUGUI description;

    public Action<bool, Building> OnCanvasClosed; 

    private int buildingIndex = 0;
    private BuildingsController buildingsController;
    private VillageResources villageResources;
    private List<Building> buildings = new List<Building>();
    private Building shownBuilding;
    private ColorConfig _colorConfig;

    private void Awake()
    {
        Assert.IsNotNull(nextButton);
        Assert.IsNotNull(previousButton);
        Assert.IsNotNull(buyButton);
        Assert.IsNotNull(buildingImage);
        Assert.IsNotNull(resourcesPriceLabel);
        Assert.IsNotNull(foodPriceLabel);
        Assert.IsNotNull(description);

        buildingsController = FindObjectOfType<BuildingsController>();
        villageResources = FindObjectOfType<VillageResources>();
        _colorConfig = ConfigController.GetConfig<ColorConfig>();
    }
    
    void Start()
    {
        buildings = buildingsController.GetAvailableBuildings();
        if(buildings == null || buildings.Count <= 0)
        {
            Debug.LogError("No buildings to buy");
            CloseCanvas();
        }

        shownBuilding = buildings[0];
        ShowBuilding();
    }

    public string PopulateStringsWithSprites(string text)
    {
        string newText = text.Replace("{f}", "<sprite=1>");
        newText = newText.Replace("{m}", "<sprite=2>");
        newText = newText.Replace("{r}", "<sprite=0>");

        return newText;
    }

    private void ShowBuilding()
    {
        if(shownBuilding != null)
        {
            buildingImage.sprite = shownBuilding.Data.BuildingImage;
            resourcesPriceLabel.text = shownBuilding.Data.ResourcesCost.ToString();
            foodPriceLabel.text = shownBuilding.Data.FoodCost.ToString();
            description.text = PopulateStringsWithSprites(shownBuilding.Data.Description);

            buyButton.enabled = true;
            foodPriceLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
            resourcesPriceLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);

            if (shownBuilding.Data.FoodCost > villageResources.Food)
            {
                foodPriceLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.Negative);
                buyButton.enabled = false;
            }
            if (shownBuilding.Data.ResourcesCost > villageResources.Resources)
            {
                resourcesPriceLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.Negative);
                buyButton.enabled = false;
            }
        }
    }

    public void ShowNextBuilding()
    {
        if(buildings.Count == buildingIndex + 1)
        {
            buildingIndex = 0;
        }
        else
        {
            buildingIndex++;
        }

        shownBuilding = buildings[buildingIndex];
        ShowBuilding();
    }

    public void ShowPreviousBuilding()
    {
        if (buildingIndex == 0)
        {
            buildingIndex = buildings.Count - 1;
        }
        else
        {
            buildingIndex--;
        }

        shownBuilding = buildings[buildingIndex];
        ShowBuilding();
    }

    public void BuyBuilding()
    {
        WorldController.worldController.lastTile?.StopHover();
        OnCanvasClosed?.Invoke(true, shownBuilding);
        Destroy(gameObject);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            CloseCanvas();
        }
    }

    public void CloseCanvas()
    {
        OnCanvasClosed?.Invoke(false, null);
        Destroy(gameObject);
    }
}
