using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DestroyBuildingCanvas : MonoBehaviour, ICanvas
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public Action<bool> OnCanvasClosed;

    private void Awake()
    {
        Assert.IsNotNull(yesButton);
        Assert.IsNotNull(noButton);

        yesButton.onClick.AddListener(HandleOnYesButtonClick);
        noButton.onClick.AddListener(HandleOnNoButtonClick);
    }

    public void CloseCanvas()
    {
        yesButton.onClick.RemoveListener(HandleOnYesButtonClick);
        noButton.onClick.RemoveListener(HandleOnNoButtonClick);
        GameController.Game.UnPauseGame();
        Destroy(gameObject);
    }

    private void HandleOnYesButtonClick()
    {
        OnCanvasClosed?.Invoke(true);
        CloseCanvas();
    }

    private void HandleOnNoButtonClick()
    {
        OnCanvasClosed?.Invoke(false);
        CloseCanvas();
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
