using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WorldController : MonoBehaviour
{
    [SerializeField] private EventCanvas eventCanvas;
    [SerializeField] private float eventCooldown;
    [SerializeField] private float cycleTime;
    [SerializeField] private Canvas gameUI;
    [SerializeField] private Canvas pauseUI;
    [SerializeField] private Canvas endGameUI;
    public Tile lastTile = null;

    private Queue<GameEvent> queuedEvents = new Queue<GameEvent>();
    private bool isEventActive = false;
    private bool isCycleActive = false;
    private bool isPaused = false;
    private bool isEventOnCooldown = false;

    //Sekcja Fabularna
    public bool hasKupalaFlower = false;
    public bool hasSword = false;
    public bool isArmoryUnlocked = false;
    public bool isWheatBetter = false;
    public bool isPerunActivated = false;
    public bool isPerunHappy = false;
    public bool isCityHallBuilt = false;
    public bool isMilBuilt = false;
    public Mill destroyMill = null;



    private float eventTime = 0;
    private int cycle = 0;
    private IEnumerator coroutine;
    public static WorldController worldController;

    private void Awake()
    {
        Assert.IsNotNull(eventCanvas);
        
        worldController = GetComponent<WorldController>();
    }
    void Start()
    {
        coroutine = StartEvent();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && !isEventActive)
        {
            ChangePauseCanvas();
        }

        if (!isEventActive && Time.time > cycleTime && !isPaused && !isCycleActive)
        {
            StartCoroutine("MakeCycle");
        }

        if(isEventOnCooldown && Time.time > eventTime + eventCooldown)
        {
            isEventOnCooldown = false;
            if(queuedEvents.Count > 0)
            {
                StartCoroutine("StartEvent");
            }
        }
    }

    private IEnumerator MakeCycle()
    {
        isCycleActive = true;
        cycle++;
        VillageResources.villageResources.ProcessCycle(cycle);
        yield return new WaitForSeconds(cycleTime);
        isCycleActive = false;
    }

    public void IncreaseWheat()
    {
        Wheat_field[] fields = GameObject.FindObjectsOfType<Wheat_field>();
        foreach(Wheat_field field in fields)
        {
            if (field.CheckIfNearMil()) VillageResources.villageResources.ChangeFoodProduction(5);
        }
    }
    public void BeginStorm()
    {
        
    }

    public void GetNamedEvent(bool devilishWell, bool perunSword, bool slayerOfTheBeast2)
    {
        GameEvent gameEvent = GameEventsController.gameEventsController.GetNamedEvent(devilishWell, perunSword, slayerOfTheBeast2);
        if (gameEvent == null) return;
        else
        {
            queuedEvents.Enqueue(gameEvent);

            if (!isEventOnCooldown)
            {
                StartCoroutine("StartEvent");
            }
        }
    }

    public void CheckEvent()
    {
        List<GameEvent> gameEvents = GameEventsController.gameEventsController.GetEvent();
        if (gameEvents == null || gameEvents.Count < 0) return;
        else
        {
            foreach(GameEvent gameEvent in gameEvents)
            {
                queuedEvents.Enqueue(gameEvent);
            }

            if (!isEventOnCooldown)
            {
                StartCoroutine("StartEvent");
            }
        }
    }

    private IEnumerator StartEvent()
    {
        
        isEventActive = true;
        var canvas = Instantiate(eventCanvas);
        canvas.ShowEvent(queuedEvents.Dequeue());
        PauseGame();
        yield return new WaitUntil(() => canvas.HasEnded);
        isEventOnCooldown = true;
        eventTime = Time.time;
        isEventActive = false;
    }

    public void EndGame()
    {
        Instantiate(endGameUI);
    }

    private void ChangePauseCanvas()
    {
        if (!isPaused)
        {
            gameUI.enabled = false;
            pauseUI.enabled = true;
            PauseGame();
            isPaused = true;
        }
        else
        {
            gameUI.enabled = true;
            pauseUI.enabled = false;
            UnPauseGame();
            isPaused = false;
        }
    }

    public void PauseGame()
    {
        if(Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
        }
    }
    public void UnPauseGame()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
    }
}
