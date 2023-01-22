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

    [Header("Food")]
    [SerializeField] int food;
    [SerializeField] private int foodConsumption;
    [SerializeField] private int foodProduction;
    [SerializeField] private int foodConsumptionCycle;
    [SerializeField] private int foodProductionCycle;

    [Header("Morale")]
    [SerializeField] int morale;
    [SerializeField] private int moraleConsumption;
    [SerializeField] private int moraleProduction;
    [SerializeField] private int moraleConsumptionCycle;
    [SerializeField] private int moraleProductionCycle;

    [SerializeField] int NormalMoraleBorder;
    [SerializeField] int LowMoraleBorder;

    [Space(5)]
    [SerializeField] GameUICanvas gameCanvas;
    [SerializeField] int buildingScore;

    public MoraleState VillageMorale
    {
        get
        {
            if (morale < LowMoraleBorder) return MoraleState.Low;
            else if (morale < NormalMoraleBorder) return MoraleState.Normal;
            else return MoraleState.High;
        }
    }

    public static VillageResources villageResources;
    public int Resources { get { return resources; } }
    public int Food { get { return food; } }
    public int Morale { get { return morale; } }
    public int BuildingScore { get { return buildingScore; } }
   
    
    //TODO look at
    private void Awake()
    {
        Assert.IsNotNull(gameCanvas);
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
            return true;
        }
        else
        {
            buildingScore += value;
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
        gameCanvas.UpdateResources(0, resources);
        gameCanvas.UpdateFood(0, food);
        gameCanvas.UpdateMorale(0, morale);

        gameCanvas.UpdateResourcesProduction(resourceProduction);
        gameCanvas.UpdateMoraleProduction(foodProduction);
        gameCanvas.UpdateFoodProduction(moraleProduction );


    }

    public bool ChangeResources(int value)
    {
        if (resources + value < 0)
        {
            return false;
        }
            
        resources += value;
        gameCanvas.UpdateResources(value, resources);
        return true;
    }

    public bool ChangeResourcesProduction(int value)
    {
        if (resourceProduction + value < 0)
        {
            return false;
        }
        resourceProduction += value;
        gameCanvas.UpdateResourcesProduction(resourceProduction);
        return true;
    }

    public bool ChangeResourcesConsumption(int value)
    {
        if (resourceConsumption + value < 0)
        {
            return false;
        }
        resourceConsumption += value;
        return true;
    }

    public bool ChangeFood(int value)
    {
        if (food + value < 0)
        {
            return false;
        }
        food += value;
        gameCanvas.UpdateFood(value, food);
        return true;
    }

    public bool ChangeFoodProduction(int value)
    {
        if (foodProduction + value < 0)
        {
            return false;
        }
        foodProduction += value;

        gameCanvas.UpdateFoodProduction(foodProduction);
        return true;
    }

    public bool ChangeFoodConsumption(int value)
    {
        if (foodConsumption + value < 0)
        {
            return false;
        }
        foodConsumption += value;
        return true;
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
            gameCanvas.UpdateMorale(value, morale);
            return true;
        }
        else if(morale + value > 100)
        {
            value = 100-morale;
            if (value == 0) return true;
            else
            {
                morale += value;
                gameCanvas.UpdateMorale(value, morale);
                return true;
            }
        }
        else
        {
            morale += value;
            gameCanvas.UpdateMorale(value, morale);
            return true;
        }
    }

    public bool ChangeMoraleProduction(int value)
    {
        if (moraleProduction + value < 0)
        {
            return false;
        }
        moraleProduction += value;
        gameCanvas.UpdateMoraleProduction(moraleProduction);
        return true;
    }

    public bool ChangeMoraleConsumption(int value)
    {
        if (moraleConsumption + value < 0)
        {
            return false;
        }
        moraleConsumption += value;
        return true;
    }
       
}
public enum MoraleState
{
    Low,
    Normal,
    High
}