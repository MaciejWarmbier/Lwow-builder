using System;
using System.Collections;
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
    [SerializeField] private Button backButton;
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI resourcesPriceLabel;
    [SerializeField] private TextMeshProUGUI foodPriceLabel;
    [SerializeField] private TextMeshProUGUI description;

    public Action<bool, Building> OnCanvasClosed; 

    private int buildingIndex = 0;
    private BuildingsController buildingsController;
    private VillageResources villageResources;
    private List<Building> buildings = new List<Building>();
    private Color defaultColor;

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
        defaultColor = Color.white;
    }
    // Start is called before the first frame update
    void Start()
    {
        buildings = buildingsController.GetAvailableBuildings();
        if(buildings == null || buildings.Count <= 0)
        {
            Debug.LogError("No buildings to buy");
            CloseCanvas();
        }

        ShowBuilding(buildings[0]);
    }

    public string ChangeStrings(string text)
    {
        string newText = text.Replace("{f}", "<sprite=1>");
        newText = newText.Replace("{m}", "<sprite=2>");
        newText = newText.Replace("{r}", "<sprite=0>");

        return newText;
    }

    private void ShowBuilding(Building building)
    {
        if(building != null)
        {
            buildingImage.sprite = building.BuildingImage;
            resourcesPriceLabel.text = building.ResourcesCost.ToString();
            foodPriceLabel.text = building.FoodCost.ToString();
            description.text = ChangeStrings(building.Description);

            buyButton.enabled = true;
            foodPriceLabel.color = defaultColor;
            resourcesPriceLabel.color = defaultColor;
            if (buildings[buildingIndex].FoodCost > villageResources.Food)
            {
                foodPriceLabel.color = Color.red;
                buyButton.enabled = false;
            }
            if (buildings[buildingIndex].ResourcesCost > villageResources.Resources)
            {
                resourcesPriceLabel.color = Color.red;
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

        ShowBuilding(buildings[buildingIndex]);
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

        ShowBuilding(buildings[buildingIndex]);
    }

    public void BuyBuilding()
    {
        buyButton.enabled = true;
        ////TODO popup
        if (buildings[buildingIndex].FoodCost > villageResources.Food)
        {
            foodPriceLabel.color = Color.red;
            OnCanvasClosed.Invoke(false, null);
            buyButton.enabled = false;
        }
        if (buildings[buildingIndex].ResourcesCost > villageResources.Resources)
        {
            resourcesPriceLabel.color = Color.red;
            OnCanvasClosed.Invoke(false, null);
            buyButton.enabled = false;
        }

        OnCanvasClosed?.Invoke(true, buildings[buildingIndex]);
        Destroy(gameObject);
    }

    // Update is called once per frame
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
