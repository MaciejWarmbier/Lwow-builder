using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePageCanvas : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
