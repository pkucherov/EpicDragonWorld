using System;
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

    public Canvas _loadingCanvas;
    public Slider _loadingBar;
    public TextMeshProUGUI _loadingPercentage;
    public MusicManager _musicManager;

    private bool _isInitialized = false; // Set to true when login scene has initialized.
    private string _accountName;
    private List<CharacterDataHolder> _characterList;
    private CharacterDataHolder _selectedCharacterData;
    private bool _isDraggingWindow = false;
    private bool _isChatBoxActive = false;
    private string _lastLoadedScene = "";

    private void Start()
    {
        Instance = this;
        // Loading canvas should be enabled.
        _loadingCanvas.enabled = true;
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
        _loadingBar.value = 0;
        _loadingPercentage.text = "0%";
        _loadingCanvas.enabled = true;
        AsyncOperation operation;
        if (!_lastLoadedScene.Equals(""))
        {
            operation = SceneManager.UnloadSceneAsync(_lastLoadedScene);
            yield return new WaitUntil(() => operation.isDone);
        }
        operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        _musicManager.PlayMusic(SceneManager.GetSceneByName(scene).buildIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingBar.value = progress;
            _loadingPercentage.text = (int)(progress * 100f) + "%";
            yield return null;
        }
        _lastLoadedScene = scene;
        _loadingCanvas.enabled = false;
    }

    public bool IsInitialized()
    {
        return _isInitialized;
    }

    public void SetInitialized(bool value)
    {
        _isInitialized = value;
    }

    public string GetAccountName()
    {
        return _accountName;
    }

    public void SetAccountName(string value)
    {
        _accountName = value;
    }

    public List<CharacterDataHolder> GetCharacterList()
    {
        return _characterList;
    }

    public void SetCharacterList(List<CharacterDataHolder> value)
    {
        _characterList = value;
    }

    public CharacterDataHolder GetSelectedCharacterData()
    {
        return _selectedCharacterData;
    }

    public void SetSelectedCharacterData(CharacterDataHolder value)
    {
        _selectedCharacterData = value;
    }

    public bool IsDraggingWindow()
    {
        return _isDraggingWindow;
    }

    public void SetDraggingWindow(bool value)
    {
        _isDraggingWindow = value;
    }

    public bool IsChatBoxActive()
    {
        return _isChatBoxActive;
    }

    public void SetChatBoxActive(bool value)
    {
        _isChatBoxActive = value;
    }

    public string GetLastLoadedScene()
    {
        return _lastLoadedScene;
    }
}