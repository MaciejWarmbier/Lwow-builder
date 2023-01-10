using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
    [SerializeField] List<GameEvent> events;

    public static GameEventsController gameEventsController;

    private void Start()
    {
        gameEventsController = GetComponent<GameEventsController>();
    }

    public GameEvent GetNamedEvent(EventType type)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].type == type && !events[i].wasUsed)
            {
                events[i].wasUsed = true;
                return events[i];
            }
        }

        return null;
    }

    public List<GameEvent> GetEvent()
    {
        List<GameEvent> possibleEvents = new List<GameEvent>();
        for (int i = 0; i< events.Count; i++)
        {
            if (!events[i].wasUsed && events[i].eventLevel <= VillageResources.villageResources.BuildingScore)
            {
                possibleEvents.Add(events[i]);
                events[i].wasUsed = true;
            }
        }
        if (possibleEvents.Count == 0)
        {
            return null;
        }

        return possibleEvents;
    }

    public void RightEventResults(GameEvent gameEvent)
    {
        if (gameEvent.type == EventType.KupalaNight)
        {
            WorldController.worldController.hasKupalaFlower = true;
        }
        else if (gameEvent.type == EventType.SlayerOfTheBeast)
        {
            WorldController.worldController.isArmoryUnlocked = true;
        }
        else if (gameEvent.type == EventType.ForestNymph)
        {
            WorldController.worldController.isWheatBetter = true;
            IncreaseWheat();
        }
        else if (gameEvent.type == EventType.AltarOfPerun)
        {
            WorldController.worldController.isPerunHappy = true;
            PerunsStormConsequences();
        }
    }

    public void WrongEventResults(GameEvent gameEvent)
    {
        if (gameEvent.type == EventType.AltarOfPerun)
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
        WorldController.worldController.destroyMill.DestroyBuilding(true);

        if (!WorldController.worldController.isPerunHappy)
        {
            for(int i=0; i< 2; i++)
            {
                var range = BuildingsController.buildingsController.listOfBuiltBuildings.Count;
                int number = Random.Range(0, range - 1);

                BuildingsController.buildingsController.listOfBuiltBuildings[number].DestroyBuilding(true);
            }
        }
    }
}
