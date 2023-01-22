using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
