using System.Collections;
using TMPro;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }

    public TextMeshProUGUI _textMessage;
    public Button _nextCharButton;
    public Button _previousCharButton;
    public Button _createCharButton;
    public Button _deleteCharButton;
    public Button _exitToLoginButton;
    public Button _enterWorldButton;
    public TextMeshProUGUI _characterName;

    private bool _waitingServer;
    private bool _characterSelected = false;
    private int _characterSelectedSlot = 0;
    private DynamicCharacterAvatar _avatar;

    private void Start()
    {
        // Set instance.
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        // In case player logouts underwater.
        RenderSettings.fogColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 500;
        RenderSettings.fogEndDistance = 1200;

        // Restore Camera Position
        Camera.main.transform.position = new Vector3(8.29f, 1.29f, 17.7f);

        // Schedule exit to login screen.
        StartCoroutine(ExitToLoginScreen());

        // Show retrieving information message.
        _textMessage.text = "Retrieving character information.";

        // Request info.
        _waitingServer = true;
        NetworkManager.SendPacket(new CharacterSelectionInfoRequest());
        // Wait until server sends existing player data.
        while (_waitingServer)
        {
            // Make sure information from the server is received.
        }

        // Show last selected character.
        if (MainManager.Instance.GetCharacterList().Count > 0)
        {
            for (int i = 0; i < MainManager.Instance.GetCharacterList().Count; i++)
            {
                // Get current character data.
                MainManager.Instance.SetSelectedCharacterData(MainManager.Instance.GetCharacterList()[i]);
                if (MainManager.Instance.GetSelectedCharacterData().IsSelected() || i == MainManager.Instance.GetCharacterList().Count - 1)
                {
                    _avatar = CharacterManager.Instance.CreateCharacter(MainManager.Instance.GetSelectedCharacterData(), 8.28f, 0.1035156f, 20.222f, 180);
                    _characterName.text = MainManager.Instance.GetSelectedCharacterData().GetName();
                    _characterSelectedSlot = i;
                    _characterSelected = true;
                    break;
                }
            }
        }
        else // In case of character deletion.
        {
            MainManager.Instance.SetSelectedCharacterData(null);
        }

        // Click listeners.
        _nextCharButton.onClick.AddListener(OnClickNextButton);
        _previousCharButton.onClick.AddListener(OnClickPreviousButton);
        _createCharButton.onClick.AddListener(OnClickCreateButton);
        _deleteCharButton.onClick.AddListener(OnClickDeleteButton);
        _exitToLoginButton.onClick.AddListener(OnClickExitButton);
        _enterWorldButton.onClick.AddListener(OnEnterWorldButton);

        // Hide retrieving information message.
        if (!_characterSelected)
        {
            _textMessage.text = "Click the create button to make a new character.";
            _deleteCharButton.gameObject.SetActive(false);
            Destroy(_avatar);
        }
        else
        {
            _enterWorldButton.Select(); // Be ready to enter via keyboard.
            _textMessage.text = "";
        }

        // Hide previous and next buttons if caharcter count is less than 2.
        if (MainManager.Instance.GetCharacterList().Count < 2)
        {
            _previousCharButton.gameObject.SetActive(false);
            _nextCharButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputManager.RETURN_DOWN)
        {
            OnEnterWorldButton();
        }
    }

    private IEnumerator ExitToLoginScreen()
    {
        yield return new WaitForSeconds(900); // Wait 15 minutes.
        OnClickExitButton();
    }

    private void OnClickExitButton()
    {
        if (_avatar != null)
        {
            Destroy(_avatar.gameObject);
        }
        MainManager.Instance.LoadScene(MainManager.LOGIN_SCENE);
    }

    private void OnClickCreateButton()
    {
        if (_avatar != null)
        {
            Destroy(_avatar.gameObject);
        }
        MainManager.Instance.LoadScene(MainManager.CHARACTER_CREATION_SCENE);
    }

    private void OnClickDeleteButton()
    {
        if (_characterSelected)
        {
            ConfirmDialog.Instance.PlayerConfirm("Delete character " + MainManager.Instance.GetSelectedCharacterData().GetName() + "?", 2);
        }
    }

    public void SetWaitingServer(bool value)
    {
        _waitingServer = value;
    }

    public void DeleteCharacter()
    {
        // Get current character data.
        CharacterDataHolder characterData = MainManager.Instance.GetSelectedCharacterData();

        // Return if no character is selected.
        if (characterData == null)
        {
            return;
        }

        // Set text message to deleting character.
        _textMessage.text = "Deleting character " + characterData.GetName() + "...";

        // Request info.
        _waitingServer = true;
        NetworkManager.SendPacket(new CharacterDeletionRequest(characterData.GetSlot()));

        // Wait until server deletes the character.
        while (_waitingServer)
        {
            // Make sure server has deleted the character.
        }

        if (_characterSelected)
        {
            Destroy(_avatar.gameObject);
        }

        // Reload everything.
        MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
    }

    private void OnClickNextButton()
    {
        if (MainManager.Instance.GetSelectedCharacterData() == null || MainManager.Instance.GetCharacterList().Count <= 1)
        {
            return;
        }
        if (_characterSelectedSlot >= MainManager.Instance.GetCharacterList().Count - 1)
        {
            _characterSelectedSlot = -1;
        }
        _characterSelectedSlot++;
        MainManager.Instance.SetSelectedCharacterData(MainManager.Instance.GetCharacterList()[_characterSelectedSlot]);
        _characterName.text = MainManager.Instance.GetSelectedCharacterData().GetName();
        NetworkManager.SendPacket(new CharacterSelectUpdate(_characterSelectedSlot));
        Destroy(_avatar.gameObject);
        _avatar = CharacterManager.Instance.CreateCharacter(MainManager.Instance.GetSelectedCharacterData(), 8.28f, 0.1035156f, 20.222f, 180);
    }

    private void OnClickPreviousButton()
    {
        if (MainManager.Instance.GetSelectedCharacterData() == null || MainManager.Instance.GetCharacterList().Count <= 1)
        {
            return;
        }
        if (_characterSelectedSlot <= 0)
        {
            _characterSelectedSlot = MainManager.Instance.GetCharacterList().Count;
        }
        _characterSelectedSlot--;
        MainManager.Instance.SetSelectedCharacterData(MainManager.Instance.GetCharacterList()[_characterSelectedSlot]);
        _characterName.text = MainManager.Instance.GetSelectedCharacterData().GetName();
        NetworkManager.SendPacket(new CharacterSelectUpdate(_characterSelectedSlot));
        Destroy(_avatar.gameObject);
        _avatar = CharacterManager.Instance.CreateCharacter(MainManager.Instance.GetSelectedCharacterData(), 8.28f, 0.1035156f, 20.222f, 180);
    }

    private void OnEnterWorldButton()
    {
        if (_characterSelected)
        {
            _characterSelected = false;
            Destroy(_avatar.gameObject);
            MainManager.Instance.LoadScene(MainManager.WORLD_SCENE);
        }
    }
}
