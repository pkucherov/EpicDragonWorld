using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/**
 * Authors: NightBR
 * Date: April 29th 2019
 */
public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance { get; private set; }

    public Dropdown _resolutionDropdown;
    public Dropdown _qualityDropdown;
    public AudioMixer _musicAudioMixer;
    public AudioMixer _sfxAudioMixer;
    public Button _closeOptionsButton;
    public Button _controlsButton;
    public Button _chatButton;
    public Button _logoutButton;
    public Button _exitGameButton;
    public Toggle _chatUseTimestamps;
    public Toggle _fullScreenToggle;
    public Slider _musicSlider;
    public Slider _sfxSlider;

    private Resolution[] _resolutions;

    private readonly static string SETTINGS_FILE_NAME = "Settings.ini";
    private readonly static string RESOLUTION_VALUE = "Resolution";
    private readonly static string QUALITY_VALUE = "Quality";
    private readonly static string FULLSCREEN_VALUE = "Fullscreen";
    private readonly static string MUSIC_VOLUME_VALUE = "MusicVolume";
    private readonly static string SFX_VOLUME_VALUE = "SfxVolume";
    private readonly static string TRUE_VALUE = "True";
    private readonly static string FALSE_VALUE = "False";

    // Storage variables
    private static int _resolutionIndexSave;
    private static int _qualityIndexSave;
    private static bool _isFullscreenSave;
    private static float _masterVolumeSave;
    private static float _gameSfxSave;
    // Chat color related.
    public Button[] _chatColorButtons;
    public Canvas _chatColorPickerCanvas;
    private int lastSelectColorButtonIndex;
    private volatile bool _useChatTimestamps = false;
    private volatile int _chatColorNormalIntValue = 16777215; // Cannot use Util.ColorToInt in packet, so we store value here.
    private volatile int _chatColorMessageIntValue = 16711760; // Cannot use Util.ColorToInt in packet, so we store value here.
    private volatile int _chatColorSystemIntValue = 16739840; // Cannot use Util.ColorToInt in packet, so we store value here.
    // Keybind related.
    public Canvas _keybindMenuCanvas;
    public TextMeshProUGUI _keybindMenuMessageText;
    private int _lastSelectKeyButtonIndex = -1;

    public MusicManager _musicManager;
    public Canvas _optionsCanvas;

    private void Start()
    {
        Instance = this;

        // Set resolution after fullscreen.
        ConfigReader configReader = new ConfigReader(SETTINGS_FILE_NAME);
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height + " (" + _resolutions[i].refreshRate + "Hz)";
            resolutionOptions.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height && _resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }
        _resolutionDropdown.AddOptions(resolutionOptions);
        _resolutionDropdown.value = configReader.GetInt(RESOLUTION_VALUE, currentResolutionIndex);
        _resolutionDropdown.RefreshShownValue();

        // Load rest of configurations.
        SetQuality(configReader.GetInt(QUALITY_VALUE, 2));
        _isFullscreenSave = configReader.GetString(FULLSCREEN_VALUE, TRUE_VALUE).Equals(TRUE_VALUE);
        SetFullscreen(_isFullscreenSave);
        _fullScreenToggle.isOn = _isFullscreenSave;

        // Set screen resolution after load settings.
        _resolutionIndexSave = _resolutionDropdown.value;
        Resolution resolution = _resolutions[_resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, _isFullscreenSave, resolution.refreshRate);

        float musicVolume = configReader.GetFloat(MUSIC_VOLUME_VALUE, 1);
        MasterVolume(musicVolume);
        _musicSlider.value = musicVolume;
        float sfxVolume = configReader.GetFloat(SFX_VOLUME_VALUE, 1);
        GameSFX(sfxVolume);
        _sfxSlider.value = sfxVolume;

        // Mute sound.
        AudioListener.volume = 0;
    }

    private void Update()
    {
        if (InputManager.ESCAPE_DOWN && !ConfirmDialog.Instance.IsConfirmDialogActive())
        {
            // If player has a target selected, cancel the target instead.
            if (MainManager.Instance.GetLastLoadedScene().Equals(MainManager.WORLD_SCENE) && WorldManager.Instance.GetTargetWorldObject() != null)
            {
                WorldManager.Instance.SetTarget(null);
                return;
            }
            ToggleOptionsMenu();
        }

        if (_lastSelectKeyButtonIndex > -1)
        {
            foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keycode) && keycode != KeyCode.Escape)
                {
                    switch (InputManager.SetKeybind(_lastSelectKeyButtonIndex, keycode))
                    {
                        case 0: // Key cannot be bound.
                            _keybindMenuMessageText.text = "Input cannot be bound. Try another key.";
                            break;

                        case 1: // Key already bound.
                            _keybindMenuMessageText.text = "Input already bound. Try another key.";
                            break;

                        case 2: // Success.
                            HideKeybindMenu();
                            // Update player options.
                            NetworkManager.SendPacket(new PlayerOptionsUpdate());
                            break;
                    }
                }
            }
        }
    }

    public bool UseChatTimestamps()
    {
        return _useChatTimestamps;
    }

    public void SetUseChatTimestamps(bool value)
    {
        _useChatTimestamps = value;
    }

    public int GetChatColorNormalIntValue()
    {
        return _chatColorNormalIntValue;
    }

    public void SetChatColorNormalIntValue(int value)
    {
        _chatColorNormalIntValue = value;
    }

    public int GetChatColorMessageIntValue()
    {
        return _chatColorMessageIntValue;
    }

    public void SetChatColorMessageIntValue(int value)
    {
        _chatColorMessageIntValue = value;
    }

    public int GetChatColorSystemIntValue()
    {
        return _chatColorSystemIntValue;
    }

    public void SetChatColorSystemIntValue(int value)
    {
        _chatColorSystemIntValue = value;
    }

    public Canvas GetKeybindMenuCanvas()
    {
        return _keybindMenuCanvas;
    }

    public Canvas GetOptionsCanvas()
    {
        return _optionsCanvas;
    }

    public void SaveConfigValues()
    {
        ConfigWriter configWriter = new ConfigWriter(SETTINGS_FILE_NAME);
        configWriter.SetInt(RESOLUTION_VALUE, _resolutionIndexSave);
        configWriter.SetInt(QUALITY_VALUE, _qualityIndexSave);
        configWriter.SetString(FULLSCREEN_VALUE, _isFullscreenSave ? TRUE_VALUE : FALSE_VALUE);
        configWriter.SetFloat(MUSIC_VOLUME_VALUE, _masterVolumeSave);
        configWriter.SetFloat(SFX_VOLUME_VALUE, _gameSfxSave);
    }

    public void SetResolution(int resolutionIndex)
    {
        _resolutionIndexSave = resolutionIndex;
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void SetVolume(float mastervolume)
    {
        _masterVolumeSave = mastervolume;
        _musicAudioMixer.SetFloat(MUSIC_VOLUME_VALUE, mastervolume);
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityIndexSave = qualityIndex;
        _qualityDropdown.value = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreenSave = isFullscreen;
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void CheckFullscreen()
    {
        if (_isFullscreenSave && !Screen.fullScreen)
        {
            SetFullscreen(true);
        }
        else if (!_isFullscreenSave && Screen.fullScreen)
        {
            SetFullscreen(false);
        }
    }

    public void OnButtonLogoutClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to logout?", 3);
    }

    public void OnButtonQuitClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to quit the game?", 1);
    }

    public void ToggleOptionsMenu()
    {
        if (_chatColorPickerCanvas.gameObject.activeSelf)
        {
            HideChatColorPicker();
            return;
        }
        if (_keybindMenuCanvas.gameObject.activeSelf)
        {
            HideKeybindMenu();
            return;
        }

        _optionsCanvas.enabled = !_optionsCanvas.enabled;
        if (!_optionsCanvas.enabled)
        {
            MainManager.Instance.SetDraggingWindow(false);
        }

        bool isInWorld = MainManager.Instance.GetLastLoadedScene().Equals(MainManager.WORLD_SCENE);
        _controlsButton.gameObject.SetActive(isInWorld);
        _chatButton.gameObject.SetActive(isInWorld);
        _logoutButton.gameObject.SetActive(isInWorld);
        _exitGameButton.gameObject.SetActive(isInWorld);
        if (isInWorld)
        {
            _chatColorButtons[0].image.color = Util.IntToColor(_chatColorNormalIntValue);
            _chatColorButtons[1].image.color = Util.IntToColor(_chatColorMessageIntValue);
            _chatColorButtons[2].image.color = Util.IntToColor(_chatColorSystemIntValue);
            _chatUseTimestamps.enabled = _optionsCanvas.enabled;
            _chatUseTimestamps.isOn = _useChatTimestamps;
        }

        SaveConfigValues();
    }

    public void HideChatColorPicker()
    {
        _chatColorPickerCanvas.gameObject.SetActive(false);
    }

    public void NormalColorButtonSelected()
    {
        lastSelectColorButtonIndex = 0;
        _chatColorPickerCanvas.gameObject.SetActive(true);
    }

    public void MessageColorButtonSelected()
    {
        lastSelectColorButtonIndex = 1;
        _chatColorPickerCanvas.gameObject.SetActive(true);
    }

    public void SystemColorButtonSelected()
    {
        lastSelectColorButtonIndex = 2;
        _chatColorPickerCanvas.gameObject.SetActive(true);
    }

    public void ChangeSelectedChatColor(Color color)
    {
        _chatColorButtons[lastSelectColorButtonIndex].image.color = color;
        _chatColorPickerCanvas.gameObject.SetActive(false);
        switch (lastSelectColorButtonIndex)
        {
            case 0:
                _chatColorNormalIntValue = Util.ColorToInt(color);
                break;

            case 1:
                _chatColorMessageIntValue = Util.ColorToInt(color);
                break;

            case 2:
                _chatColorSystemIntValue = Util.ColorToInt(color);
                break;
        }

        // Update player options.
        NetworkManager.SendPacket(new PlayerOptionsUpdate());
    }

    public void ResetChatColors()
    {
        lastSelectColorButtonIndex = 0;
        ChangeSelectedChatColor(new Color(255, 255, 255));
        lastSelectColorButtonIndex = 1;
        ChangeSelectedChatColor(new Color(255, 0, 80));
        lastSelectColorButtonIndex = 2;
        ChangeSelectedChatColor(new Color(255, 110, 0));

        _useChatTimestamps = false;
    }

    public void ToggleTimestampUse()
    {
        _useChatTimestamps = !_useChatTimestamps;
        // Update player options.
        NetworkManager.SendPacket(new PlayerOptionsUpdate());
    }

    public void HideKeybindMenu()
    {
        _keybindMenuCanvas.gameObject.SetActive(false);
        _lastSelectKeyButtonIndex = -1;
    }

    public void ShowKeybindMenu(int index)
    {
        _lastSelectKeyButtonIndex = index;
        _keybindMenuCanvas.gameObject.SetActive(true);
    }

    public void OnButtonCloseClick()
    {
        ToggleOptionsMenu();
    }

    // Slider Volume Control Section
    public void MasterVolume(float value)
    {
        _masterVolumeSave = value;
        _musicAudioMixer.SetFloat(MUSIC_VOLUME_VALUE, Mathf.Log10(value) * 20);
    }

    public void GameSFX(float value)
    {
        _gameSfxSave = value;
        _sfxAudioMixer.SetFloat(SFX_VOLUME_VALUE, Mathf.Log10(value) * 20);
    }

    public float GetSfxVolume()
    {
        return _gameSfxSave;
    }

    // Mute Volume Section
    public void MuteMaster()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void ClearVolume()
    {
        _musicAudioMixer.ClearFloat("mastervolume");
    }
}
