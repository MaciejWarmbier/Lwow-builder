using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameEventsController : MonoBehaviour, IController
{
    [SerializeField] List<GameEvent> events;
    [SerializeField] private float eventCooldown;
    
    private ResourcesController _resourcesController;
    private CanvasController _canvasController;
    private PlotController _plotController;
    private BuildingsController _buildingsController;

    private bool isEventActive = false;
    private bool isEventOnCooldown = false;
    private float eventEndTime = 0;

    private void Update()
    {
        if (isEventOnCooldown && Time.time > eventEndTime + eventCooldown && !GameController.Game.isPaused)
        {
            isEventOnCooldown = false;
            CheckEvent();
        }
    }

    public void CheckEvent(int i =0)
    {
        GameEvent gameEvent = GetEvent();
        if (gameEvent == null) return;
        else
        {
            if (!isEventOnCooldown && !isEventActive)
            {
                StartCoroutine(StartEvent(gameEvent));
            }
        }
    }

    private IEnumerator StartEvent(GameEvent gameEvent)
    {
        FindObjectOfType<GameUICanvas>().HideMessageBar();
        gameEvent.wasUsed = true;
        isEventActive = true;
        _canvasController.ShowEventCanvas(gameEvent);
        yield return new WaitUntil(() => !_canvasController.isCanvasActive);

        isEventOnCooldown = true;
        eventEndTime = Time.time;
        isEventActive = false;
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
        CheckEvent();
    }

    public void LockNamedEvent(EventType type)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].type == type && !events[i].wasUsed)
            {
                events[i].isUnlocked = false;
                return;
            }
        }
    }

    private GameEvent GetEvent()
    {
        GameEvent possibleGameEvent = null; 
        for (int i = 0; i< events.Count; i++)
        {
            if (!events[i].wasUsed && events[i].eventLevel <= _resourcesController.BuildingScore && events[i].isUnlocked)
            {
                if (events[i].minimumResources.resources <= _resourcesController.Resources
                    && events[i].minimumResources.food <= _resourcesController.Food
                    && events[i].minimumResources.morale <= _resourcesController.Morale)
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
            if (possibleGameEvent.minimumResources.resources > _resourcesController.Resources) message = message + "{r} ";
            if (possibleGameEvent.minimumResources.food > _resourcesController.Food) message = message + "{f} ";
            if (possibleGameEvent.minimumResources.morale > _resourcesController.Morale) message = message + "{m} ";

            message = message + "to start next event.";

            _canvasController.GameCanvas.ShowMessageBar(message);
        }

        return null;
    }

    public void RightEventResults(GameEvent gameEvent)
    {
        switch (gameEvent.type)
        {
            case EventType.KupalaNight:
                _plotController.hasKupalaFlower = true;
                _canvasController.GameCanvas.ShowFlower();
                break;
            case EventType.SlayerOfTheBeast:
                _plotController.isArmoryUnlocked = true;
                break;
            case EventType.ForestNymph:
                _plotController.isWheatBetter = true;
                IncreaseWheat();
                break;
            case EventType.StormOfPerun:
                _plotController.isPerunHappy = true;
                PerunsStormConsequences();
                break;
            case EventType.AltarOfPerun:
                _plotController.hasSword = true;
                _canvasController.GameCanvas.ShowSword();
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
            _resourcesController.ChangeFoodProduction(mills * 5);
        }
    }

    public void PerunsStormConsequences()
    {
        _plotController.isPerunActivated = true;
        _plotController.destroyMill.DestroyBuilding(true);

        if (!_plotController.isPerunHappy)
        {
            for(int i=0; i< 2; i++)
            {
                var range = _buildingsController.listOfBuiltBuildings.Count;
                int number = Random.Range(0, range - 1);

                _buildingsController.listOfBuiltBuildings[number].DestroyBuilding(false);
            }
        }
    }

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        _resourcesController = GameController.Game.GetController<ResourcesController>();
        _canvasController = GameController.Game.GetController<CanvasController>();
        _plotController = GameController.Game.GetController<PlotController>();
        _buildingsController = GameController.Game.GetController<BuildingsController>();

        GameController.Game.OnCycle += CheckEvent;
        return true;
    }
}
