using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
