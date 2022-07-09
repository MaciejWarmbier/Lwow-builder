using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WorldController : MonoBehaviour
{
    [SerializeField] EventCanvas eventCanvas;
    [SerializeField] float eventTimer;
    [SerializeField] private Canvas gameUI;
    [SerializeField] private Canvas pauseUI;

    private bool isEventActive = false;
    private bool isPaused = false;
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
        if (!isEventActive && Time.time > eventTimer && !isPaused)
        {
            StartCoroutine("StartEvent");
        }

        if (Input.GetKeyUp(KeyCode.P) && !isEventActive)
        {
            ChangePauseCanvas();
        }
        Debug.Log(Time.time);
    }

    private IEnumerator StartEvent()
    {
        isEventActive = true;
        GameEvent gameEvent = GameEventsController.gameEventsController.GetEvent();
        var canvas = Instantiate(eventCanvas);
        canvas.ShowEvent(gameEvent);
        PauseGame();
        yield return new WaitUntil(() => canvas.HasEnded);

        yield return new WaitForSeconds(eventTimer);

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
