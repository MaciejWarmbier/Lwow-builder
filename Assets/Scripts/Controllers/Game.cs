using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Game : MonoBehaviour 
{
    [SerializeField] int cycleTime;
    public Action<int> OnCycle;
    public static List<IController> Controllers { get; private set; } = new List<IController>();
    public bool isPaused { get; private set; }
    private bool isCycleActive = false;
    private int cycle = 0;

    private void Awake()
    {
        isCycleActive = false;
        cycle = 0;
    }

    public void AddControllers(params IController[] controllersToAdd)
    {
        foreach (IController s in controllersToAdd)
        {
            Controllers.Add(s);
        }
    }

    public async Task<bool> InitializeControllers()
    {
        var combinedTask = await Task.WhenAll(Controllers.Select(x => x.Initialize()));

        foreach(var task in combinedTask)
        {
            if (!task) return false;
        }

        return true;
    }

    public T GetController<T>() where T : class, IController
    {
        var controller = Controllers.FirstOrDefault(x => x is T);
        if (controller == null)
        {
            throw new Exception($"Controller typeof:{typeof(T)} not found!");
        }
        return controller as T;
    }

    public void Update()
    {
        if (Time.time > cycleTime && !isPaused && !isCycleActive)
        {
            isCycleActive = true;
            cycle++;
            OnCycle?.Invoke(cycle);
            StartCoroutine(WaitForCycleEnd(cycleTime));
        }
    }

    private IEnumerator WaitForCycleEnd(int time)
    {
        yield return new WaitForSeconds(time);
        isCycleActive = false;
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
