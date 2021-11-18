using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 22th 2018
 */
public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    public Button _loginButton;
    public Button _quitButton;
    public Button _optionsButton;
    public TMP_InputField _accountNameField;
    public TMP_InputField _passwordField;
    public TextMeshProUGUI _messageText;
    public TextMeshProUGUI _versionText;

    private int _status;
    private bool _authenticating;

    private void Start()
    {
        // In case player logouts underwater.
        RenderSettings.fogColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 500;
        RenderSettings.fogEndDistance = 1200;

        // Restore Camera Position
        Camera.main.transform.position = new Vector3(0f, 1f, 0.95f);

        // Make sure options manager has set fullscreen.
        OptionsManager.Instance.CheckFullscreen();

        // If player exits to login screen, authentication must be repeated.
        NetworkManager.DisconnectFromServer();
        // In case player was forced kicked from the game.
        if (NetworkManager.IsForcedDisconnection())
        {
            _messageText.text = "You have been kicked from the game.";
            NetworkManager.SetForcedDisconnection(false);
        }
        // In case connection was lost.
        if (NetworkManager.IsUnexpectedDisconnection())
        {
            _messageText.text = "Could not communicate with the server.";
            NetworkManager.SetUnexpectedDisconnection(false);
        }

        // Set instance.
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        // Display version text.
        _versionText.text = "Version " + (VersionConfigurations.CLIENT_VERSION % 1 == 0 ? VersionConfigurations.CLIENT_VERSION + ".0" : VersionConfigurations.CLIENT_VERSION.ToString());

        // Button listeners.
        _loginButton.onClick.AddListener(OnButtonLoginClick);
        _optionsButton.onClick.AddListener(OnButtonOptionsClick);
        _quitButton.onClick.AddListener(OnButtonQuitClick);

        // One time opperations.
        if (!MainManager.Instance.IsInitialized())
        {
            // Example using command line arguments.
            // EpicDragonWorld -ip 127.0.0.1 -port 5055 -account peter -password 12345

            // Arguments can be used individually as bellow.
            // EpicDragonWorld -ip 127.0.0.1
            // Or
            // EpicDragonWorld -ip 127.0.0.1 -account peter -password 12345

            // Process command line arguments.
            string ip = CommandLineArguments.Get("-ip");
            if (ip != null)
            {
                NetworkConfigurations.SERVER_IP = ip;
            }
            string port = CommandLineArguments.Get("-port");
            if (port != null && int.TryParse(port, out int portNumber))
            {
                NetworkConfigurations.SERVER_PORT = portNumber;
            }
            string account = CommandLineArguments.Get("-account");
            if (account != null)
            {
                _accountNameField.text = account;
            }
            string password = CommandLineArguments.Get("-password");
            if (password != null)
            {
                _passwordField.text = password;
            }

            // Attempt to auto connect when possible.
            if (account != null && password != null)
            {
                StartCoroutine(AttemptAutoLogin());
            }
        }

        // At this point client has initialized.
        MainManager.Instance.SetInitialized(true);
    }

    private IEnumerator AttemptAutoLogin()
    {
        // Wait fot scene to complete Start, so it can properly be unloaded.
        yield return new WaitForSeconds(0.001f);

        // Emulate clicking the login button.
        OnButtonLoginClick();
    }

    public void SetStatus(int value)
    {
        _status = value;
    }

    private void OnButtonLoginClick()
    {
        // Disable buttons.
        DisableButtons();

        // Store login information.
        string account = _accountNameField.text;
        string password = _passwordField.text;

        // Input field checks.
        if (account == "")
        {
            _messageText.text = "Please enter your account name.";
            EnableButtons();
            return;
        }
        if (password == "")
        {
            _messageText.text = "Please enter your password.";
            EnableButtons();
            return;
        }
        if (account.Length < 2)
        {
            _messageText.text = "Account name length is too short.";
            EnableButtons();
            return;
        }
        if (password.Length < 2)
        {
            _messageText.text = "Password length is too short.";
            EnableButtons();
            return;
        }

        // Try to connect to server.
        if (!NetworkManager.ConnectToServer())
        {
            _messageText.text = "Could not communicate with the server.";
            EnableButtons();
            return;
        }

        // Authenticate.
        _messageText.text = "Authenticating...";
        _status = -1;
        NetworkManager.SendPacket(new AccountAuthenticationRequest(account, password));

        // Wait for result.
        _authenticating = true;
        while (_authenticating)
        {
            switch (_status)
            {
                case 0:
                    _messageText.text = "Account does not exist.";
                    _authenticating = false;
                    break;

                case 1:
                    _messageText.text = "Account is banned.";
                    _authenticating = false;
                    break;

                case 2:
                    _messageText.text = "Account requires activation.";
                    _authenticating = false;
                    break;

                case 3:
                    _messageText.text = "Wrong password.";
                    _authenticating = false;
                    break;

                case 4:
                    _messageText.text = "Account is already connected.";
                    _authenticating = false;
                    break;

                case 5:
                    _messageText.text = "Too many online players, please try again later.";
                    _authenticating = false;
                    break;

                case 6:
                    _messageText.text = "Incorrect client version.";
                    _authenticating = false;
                    break;

                case 7:
                    _messageText.text = "Server is not available.";
                    _authenticating = false;
                    break;

                case 100:
                    _messageText.text = "Authenticated.";
                    _authenticating = false;
                    break;
            }
        }

        // Go to player selection screen.
        if (_status == 100)
        {
            MainManager.Instance.SetAccountName(account);
            MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
        }
        else // Enable buttons.
        {
            NetworkManager.DisconnectFromServer();
            EnableButtons();
        }
    }

    private void DisableButtons()
    {
        _messageText.text = "Connecting..."; // Clean any old messages.
        _loginButton.enabled = false;
        _accountNameField.enabled = false;
        _passwordField.enabled = false;
    }

    private void EnableButtons()
    {
        _loginButton.enabled = true;
        _accountNameField.enabled = true;
        _passwordField.enabled = true;
    }

    private void OnButtonOptionsClick()
    {
        OptionsManager.Instance.ToggleOptionsMenu();
    }

    private void OnButtonQuitClick()
    {
        ConfirmDialog.Instance.PlayerConfirm("Are you sure you want to quit the game?", 1);
    }
}
