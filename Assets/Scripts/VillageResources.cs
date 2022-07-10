using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class VillageResources : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] int resources;
    [SerializeField] private int resourceConsumption;
    [SerializeField] private int resourceProduction;
    [SerializeField] private int resourceConsumptionCycle;
    [SerializeField] private int resourceProductionCycle;
    [SerializeField] TextMeshProUGUI resourcesLabel;

    [Header("Food")]
    [SerializeField] int food;
    [SerializeField] private int foodConsumption;
    [SerializeField] private int foodProduction;
    [SerializeField] private int foodConsumptionCycle;
    [SerializeField] private int foodProductionCycle;
    [SerializeField] TextMeshProUGUI foodLabel;

    [Header("Morale")]
    [SerializeField] int morale;
    [SerializeField] private int moraleConsumption;
    [SerializeField] private int moraleProduction;
    [SerializeField] private int moraleConsumptionCycle;
    [SerializeField] private int moraleProductionCycle;
    [SerializeField] TextMeshProUGUI moraleLabel;

    [SerializeField] int buildingScore;


    public static VillageResources villageResources;
    public int Resources { get { return resources; } }
    public int Food { get { return food; } }
    public int Morale { get { return morale; } }
    public int BuildingScore { get { return buildingScore; } }
   
    

    private void Awake()
    {
        Assert.IsNotNull(resourcesLabel);
        Assert.IsNotNull(foodLabel);
        Assert.IsNotNull(moraleLabel);

        villageResources = GetComponent<VillageResources>();
    }

    public bool ChangeBuildingScore(int value)
    {
        if (value < 0)
        {
            if (buildingScore + value < 0)
            {
                return false;
            }
            buildingScore += value;
            UpdateLabels();
            return true;
        }
        else
        {
            buildingScore += value;
            UpdateLabels();
            return true;
        }
    }

    public void ProcessCycle(int cycle)
    {
        if(cycle%foodConsumptionCycle == 0)
        {
            ChangeFood(-foodConsumption);
        }

        if (cycle % resourceConsumptionCycle == 0)
        {
            ChangeResources(-resourceConsumption);
        }

        if (cycle % moraleConsumptionCycle == 0)
        {
            ChangeMorale(-moraleConsumption);
        }

        if (cycle % foodProductionCycle == 0)
        {
            ChangeFood(foodProduction);
        }

        if (cycle % resourceProductionCycle == 0)
        {
            ChangeResources(resourceProduction);
        }

        if (cycle % moraleProductionCycle == 0)
        {
            ChangeMorale(moraleProduction);
        }
    }

    private void Start()
    {
        UpdateLabels();
    }

    public bool ChangeResources(int value)
    {
        if (value < 0)
        {
            if (resources + value < 0)
            {
                return false;
            }
            resources += value;
            UpdateLabels();
            return true;
        }
        else
        {
            resources += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeResourcesProduction(int value)
    {
        if (value < 0)
        {
            if (resourceProduction + value < 0)
            {
                return false;
            }
            resourceProduction += value;
            UpdateLabels();
            return true;
        }
        else
        {
            resourceProduction += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeResourcesConsumption(int value)
    {
        if (value < 0)
        {
            if (resourceConsumption + value < 0)
            {
                return false;
            }
            resourceConsumption += value;
            UpdateLabels();
            return true;
        }
        else
        {
            resourceConsumption += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeFood(int value)
    {
        if(value < 0)
        {
            if (food + value < 0)
            {
                return false;
            }
            food += value;
            UpdateLabels();
            return true;
        }
        else
        {
            food += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeFoodProduction(int value)
    {
        if (value < 0)
        {
            if (foodProduction + value < 0)
            {
                return false;
            }
            foodProduction += value;
            UpdateLabels();
            return true;
        }
        else
        {
            foodProduction += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeFoodConsumption(int value)
    {
        if (value < 0)
        {
            if (foodConsumption + value < 0)
            {
                return false;
            }
            foodConsumption += value;
            UpdateLabels();
            return true;
        }
        else
        {
            foodConsumption += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeMorale(int value)
    {
        if (value < 0)
        {
            if (WorldController.worldController.isCityHallBuilt) value = Mathf.FloorToInt(value / 2);
            if (morale + value < 0)
            {
                WorldController.worldController.EndGame();
                return false;
            }
            morale += value;
            UpdateLabels();
            return true;
        }
        else
        {
            morale += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeMoraleProduction(int value)
    {
        if (value < 0)
        {
            if (moraleProduction + value < 0)
            {
                return false;
            }
            moraleProduction += value;
            UpdateLabels();
            return true;
        }
        else
        {
            moraleProduction += value;
            UpdateLabels();
            return true;
        }
    }

    public bool ChangeMoraleConsumption(int value)
    {
        if (value < 0)
        {
            if (moraleConsumption + value < 0)
            {
                
                return false;
            }
            moraleConsumption += value;
            UpdateLabels();
            return true;
        }
        else
        {
            moraleConsumption += value;
            UpdateLabels();
            return true;
        }
    }

    public void UpdateLabels()
    {
        foodLabel.text = food.ToString();
        moraleLabel.text = morale.ToString();
        resourcesLabel.text = resources.ToString();
    }
}
