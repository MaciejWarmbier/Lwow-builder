using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
    [SerializeField] List<GameEvent> events;

    public static GameEventsController gameEventsController;

    private void Start()
    {
        gameEventsController = GetComponent<GameEventsController>();
    }

    public GameEvent GetNamedEvent(bool devilishWell, bool perunSword, bool slayerOfTheBeast2)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (devilishWell)
            {
                if (!events[i].wasUsed && events[i].isDevilishWell)
                {
                    events[i].wasUsed = true;
                    return events[i];
                }
            }

            if (perunSword)
            {
                if (!events[i].wasUsed && events[i].isPerunSword)
                {
                    events[i].wasUsed = true;
                    return events[i];
                }
            }

            if (slayerOfTheBeast2)
            {
                if (!events[i].wasUsed && events[i].isSlayerOfTheBeast2)
                {
                    events[i].wasUsed = true;
                    return events[i];
                }
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
            //Debug.LogError("No possible events to get!");
            return null;
        }

        return possibleEvents;
    }
}
