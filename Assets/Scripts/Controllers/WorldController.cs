using System.Collections;
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
    public float clickCooldown;
    public bool isPaused = false;

    [Header("Fabularne")]
    public bool hasKupalaFlower = false;
    public bool hasSword = false;
    public bool isArmoryUnlocked = false;
    public bool isWheatBetter = false;
    public bool isPerunActivated = false;
    public bool isPerunHappy = false;
    public bool isCityHallBuilt = false;
    public bool isMillBuilt = false;
    public Mill destroyMill = null;

    private float eventEndTime = 0;
    private int cycle = 0;
    private bool isEventActive = false;
    private bool isCycleActive = false;
    private bool isEventOnCooldown = false;

    public static WorldController worldController;

    private void Awake()
    {
        Assert.IsNotNull(eventCanvas);
        Assert.IsNotNull(gameUI);
        Assert.IsNotNull(pauseUI);
        Assert.IsNotNull(endGameUI);
        
        worldController = GetComponent<WorldController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && !isEventActive)
        {
            ChangePauseCanvas();
        }

        if (!isEventActive && Time.time > cycleTime && !isPaused && !isCycleActive)
        {
            StartCoroutine(MakeCycle());
        }

        if(isEventOnCooldown && Time.time > eventEndTime + eventCooldown && !isPaused)
        {
            isEventOnCooldown = false;
            CheckEvent();
        }
    }

    private IEnumerator MakeCycle()
    {
        isCycleActive = true;
        cycle++;
        VillageResources.villageResources.ProcessCycle(cycle);
        CheckEvent();
        yield return new WaitForSeconds(cycleTime);
        isCycleActive = false;
    }

    public void GetNamedEvent(EventType type)
    {
        GameEventsController.gameEventsController.UnlockNamedEvent(type);
        CheckEvent();
    }

    public void CheckEvent()
    {
        GameEvent gameEvent = GameEventsController.gameEventsController.GetEvent();
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
        var canvas = Instantiate(eventCanvas);
        canvas.ShowEvent(gameEvent);
        PauseGame();
        yield return new WaitUntil(() => canvas.EventHasEnded);

        isEventOnCooldown = true;
        eventEndTime = Time.time;
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
        isPaused = true;
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
        }
    }

    public void UnPauseGame()
    {
        isPaused = false;
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
    }
}
