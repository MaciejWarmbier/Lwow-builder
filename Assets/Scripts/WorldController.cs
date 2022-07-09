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

    private Queue<GameEvent> queuedEvents = new Queue<GameEvent>();
    private bool isEventActive = false;
    private bool isCycleActive = false;
    private bool isPaused = false;
    private bool isEventOnCooldown = false;

    public bool isPerunActivated = false;
    public bool isCityHallBuilt = false;


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
