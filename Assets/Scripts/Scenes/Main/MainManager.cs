using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 22th 2018
 */
public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public static readonly string LOGIN_SCENE = "Login";
    public static readonly string CHARACTER_SELECTION_SCENE = "CharacterSelection";
    public static readonly string CHARACTER_CREATION_SCENE = "CharacterCreation";
    public static readonly string WORLD_SCENE = "World";

    public Canvas loadingCanvas;
    public Slider loadingBar;
    public TextMeshProUGUI loadingPercentage;
    public MusicManager musicManager;

    [HideInInspector]
    public bool hasInitialized = false; // Set to true when login scene has initialized.
    [HideInInspector]
    public string accountName;
    [HideInInspector]
    public List<CharacterDataHolder> characterList;
    [HideInInspector]
    public CharacterDataHolder selectedCharacterData;
    [HideInInspector]
    public bool isDraggingWindow = false;
    [HideInInspector]
    public bool isChatBoxActive = false;

    public string lastLoadedScene = "";

    private void Start()
    {
        Instance = this;
        // Loading canvas should be enabled.
        loadingCanvas.enabled = true;
        // Initialize network manager.
        new NetworkManager();
        // Load first scene.
        LoadScene(LOGIN_SCENE);
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene));
    }

    private IEnumerator LoadSceneCoroutine(string scene)
    {
        loadingBar.value = 0;
        loadingPercentage.text = "0%";
        loadingCanvas.enabled = true;
        AsyncOperation operation;
        if (!lastLoadedScene.Equals(""))
        {
            operation = SceneManager.UnloadSceneAsync(lastLoadedScene);
            yield return new WaitUntil(() => operation.isDone);
        }
        operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        musicManager.PlayMusic(SceneManager.GetSceneByName(scene).buildIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            loadingPercentage.text = (int)(progress * 100f) + "%";
            yield return null;
        }
        lastLoadedScene = scene;
        loadingCanvas.enabled = false;
    }
}