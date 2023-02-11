using System.Threading.Tasks;
using UnityEngine;

public class ResourcesController : MonoBehaviour, IController
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
    [SerializeField] int buildingScore;

    private CanvasController _canvasController;
    private PlotController _plotController;

    public MoraleState VillageMorale
    {
        get
        {
            if (morale < LowMoraleBorder) return MoraleState.Low;
            else if (morale < NormalMoraleBorder) return MoraleState.Normal;
            else return MoraleState.High;
        }
    }
    public int Resources { get { return resources; } }
    public int Food { get { return food; } }
    public int Morale { get { return morale; } }
    public int BuildingScore { get { return buildingScore; } }
   
    
    //TODO stacking markers
    //TODO penalty for <0 stuff

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

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        _canvasController = GameController.Game.GetController<CanvasController>();
        _plotController = GameController.Game.GetController<PlotController>();

        _canvasController.GameCanvas.UpdateResources(0, resources);
        _canvasController.GameCanvas.UpdateFood(0, food);
        _canvasController.GameCanvas.UpdateMorale(0, morale);

        _canvasController.GameCanvas.UpdateResourcesProduction(resourceProduction);
        _canvasController.GameCanvas.UpdateMoraleProduction(foodProduction);
        _canvasController.GameCanvas.UpdateFoodProduction(moraleProduction );

        GameController.Game.OnCycle += ProcessCycle;
        Debug.Log("Village resources Initialized");
        return true;
    }

    public bool ChangeResources(int value)
    {
        if (resources + value < 0)
        {
            return false;
        }
            
        resources += value;
        _canvasController.GameCanvas.UpdateResources(value, resources);
        return true;
    }

    public bool ChangeResourcesProduction(int value)
    {
        if (resourceProduction + value < 0)
        {
            return false;
        }
        resourceProduction += value;
        _canvasController.GameCanvas.UpdateResourcesProduction(resourceProduction);
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
        _canvasController.GameCanvas.UpdateFood(value, food);
        return true;
    }

    public bool ChangeFoodProduction(int value)
    {
        if (foodProduction + value < 0)
        {
            return false;
        }
        foodProduction += value;

        _canvasController.GameCanvas.UpdateFoodProduction(foodProduction);
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
            if (_plotController.isCityHallBuilt) value = Mathf.FloorToInt(value / 2);
            if (morale + value < 0)
            {
                _plotController.EndGame();
                return false;
            }
            morale += value;
            _canvasController.GameCanvas.UpdateMorale(value, morale);
            return true;
        }
        else if(morale + value > 100)
        {
            value = 100-morale;
            if (value == 0) return true;
            else
            {
                morale += value;
                _canvasController.GameCanvas.UpdateMorale(value, morale);
                return true;
            }
        }
        else
        {
            morale += value;
            _canvasController.GameCanvas.UpdateMorale(value, morale);
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
        _canvasController.GameCanvas.UpdateMoraleProduction(moraleProduction);
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