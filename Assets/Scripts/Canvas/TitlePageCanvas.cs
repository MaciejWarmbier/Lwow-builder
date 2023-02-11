using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePageCanvas : MonoBehaviour
{
    [SerializeField] List<Sprite> LoadingSprites;
    [SerializeField] GameObject PlayButtonObject;
    [SerializeField] Button PlayButton;
    [SerializeField] GameObject LoadingObject;
    [SerializeField] Image LoadingImage;

    private void Awake()
    {
        LoadingObject.SetActive(false);
        PlayButton.onClick.AddListener(()=> LoadScene(1));
    }

    private void OnDestroy()
    {
        PlayButton.onClick.RemoveListener(() => LoadScene(1));
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        PlayButtonObject.SetActive(false);
        LoadingObject.SetActive(true);

        int spriteIndex = 0;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        LoadingObject.SetActive(true);
        while (!operation.isDone)
        {
            spriteIndex++;
            if(spriteIndex == LoadingSprites.Count) spriteIndex= 0;
            LoadingImage.sprite = LoadingSprites[spriteIndex];
            yield return new WaitForSeconds(0.05f);
        }

        spriteIndex++;
        if (spriteIndex == LoadingSprites.Count) spriteIndex = 0;
        LoadingImage.sprite = LoadingSprites[spriteIndex];
        yield return new WaitForSeconds(0.05f);


        var bootstrap = FindObjectOfType<Bootstrap>();

        while (!bootstrap.isInitialized)
        {
            spriteIndex++;
            if (spriteIndex == LoadingSprites.Count) spriteIndex = 0;
            LoadingImage.sprite = LoadingSprites[spriteIndex];
            yield return new WaitForSeconds(0.05f);
        }

        spriteIndex++;
        if (spriteIndex == LoadingSprites.Count) spriteIndex = 0;
        LoadingImage.sprite = LoadingSprites[spriteIndex];
        yield return new WaitForSeconds(0.05f);

        Debug.Log("Scene Fully Loaded");
        GameController.Game.GetController<CanvasController>().OpenTutorialCanvas();

        Destroy(this.gameObject);
    }
}
