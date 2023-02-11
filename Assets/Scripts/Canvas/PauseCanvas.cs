using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour, ICanvas
{
    public Action OnClose;
    public Action OnTutorialButtonClick;
    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        SetActive(false);
        OnClose?.Invoke();
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void ShowTutorial()
    {
        OnTutorialButtonClick?.Invoke();
    }

    public void CloseCanvas()
    {
        ContinueGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
