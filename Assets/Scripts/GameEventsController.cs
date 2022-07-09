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

    public GameEvent GetEvent()
    {
        List<GameEvent> possibleEvents = new List<GameEvent>();
        foreach (GameEvent gameEvent in events)
        {
            if(!gameEvent.wasUsed && gameEvent.eventLevel == VillageResources.villageResources.Level)
            {
                possibleEvents.Add(gameEvent);
            }
        }
        if (possibleEvents.Count == 0)
        {
            Debug.LogError("No possible events to get!");
            return null;
        }

        int randomIndex = Random.Range(0, possibleEvents.Count-1);

        return possibleEvents[randomIndex];
    }
}
