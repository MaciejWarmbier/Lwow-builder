using System.Collections.Generic;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
    [SerializeField] List<GameEvent> events;

    public static GameEventsController gameEventsController;
    private GameUICanvas gameUICanvas;

    private void Start()
    {
        gameEventsController = GetComponent<GameEventsController>();
        gameUICanvas = FindObjectOfType<GameUICanvas>();
    }

    public void UnlockNamedEvent(EventType type)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].type == type && !events[i].wasUsed)
            {
                events[i].isUnlocked = true;
                return;
            }
        }
    }

    public GameEvent GetEvent()
    {
        GameEvent possibleGameEvent = null; 
        for (int i = 0; i< events.Count; i++)
        {
            if (!events[i].wasUsed && events[i].eventLevel <= VillageResources.villageResources.BuildingScore && events[i].isUnlocked)
            {
                if (events[i].minimumResources.resources <= VillageResources.villageResources.Resources
                    && events[i].minimumResources.food <= VillageResources.villageResources.Food
                    && events[i].minimumResources.morale <= VillageResources.villageResources.Morale)
                {
                    return events[i];
                }
                else
                {
                    possibleGameEvent = events[i];
                }
            }
        }

        if(possibleGameEvent != null)
        {
            string message = "You need a bit more ";
            if (possibleGameEvent.minimumResources.resources > VillageResources.villageResources.Resources) message = message + "{r} ";
            if (possibleGameEvent.minimumResources.food > VillageResources.villageResources.Food) message = message + "{f} ";
            if (possibleGameEvent.minimumResources.morale > VillageResources.villageResources.Morale) message = message + "{m} ";

            message = message + "to start next event.";

            gameUICanvas.ShowMessageBar(message);
        }

        return null;
    }

    public void RightEventResults(GameEvent gameEvent)
    {
        switch (gameEvent.type)
        {
            case EventType.KupalaNight:
                WorldController.worldController.hasKupalaFlower = true;
                gameUICanvas.ShowFlower();
                break;
            case EventType.SlayerOfTheBeast:
                WorldController.worldController.isArmoryUnlocked = true;
                break;
            case EventType.ForestNymph:
                WorldController.worldController.isWheatBetter = true;
                IncreaseWheat();
                break;
            case EventType.StormOfPerun:
                WorldController.worldController.isPerunHappy = true;
                PerunsStormConsequences();
                break;
            case EventType.AltarOfPerun:
                WorldController.worldController.hasSword = true;
                gameUICanvas.ShowSword();
                break;
            default:
                break;
        }
    }

    public void WrongEventResults(GameEvent gameEvent)
    {
        if (gameEvent.type == EventType.StormOfPerun)
        {
            PerunsStormConsequences();
        }
    }

    public void IncreaseWheat()
    {
        Wheat_field[] fields = GameObject.FindObjectsOfType<Wheat_field>();
        foreach (Wheat_field field in fields)
        {
            int mills = field.CheckForNeighbor(BuildingConfig.BuildingType.Mill);
            VillageResources.villageResources.ChangeFoodProduction(mills * 5);
        }
    }

    public void PerunsStormConsequences()
    {
        WorldController.worldController.isPerunActivated = true;
        WorldController.worldController.destroyMill.DestroyBuilding(true);

        if (!WorldController.worldController.isPerunHappy)
        {
            for(int i=0; i< 2; i++)
            {
                var range = BuildingsController.buildingsController.listOfBuiltBuildings.Count;
                int number = Random.Range(0, range - 1);

                BuildingsController.buildingsController.listOfBuiltBuildings[number].DestroyBuilding(false);
            }
        }
    }
}
