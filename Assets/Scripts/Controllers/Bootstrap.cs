using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public bool isInitialized = false;

    private async void Awake()
    {
        GameController.Game = GetComponent<Game>();

        GameController.Game.AddControllers(
            FindObjectOfType<PlotController>(),
            FindObjectOfType<GridManager>(),
            FindObjectOfType<ResourcesController>(),
            FindObjectOfType<BuildingsController>(),
            FindObjectOfType<CanvasController>(),
            FindObjectOfType<GameEventsController>()
            );

        isInitialized = await GameController.Game.InitializeControllers();
    }
}
