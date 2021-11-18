using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 22th 2018
 */
public class ConfirmDialog : MonoBehaviour
{
    public static ConfirmDialog Instance { get; private set; }

    public Canvas _canvas;
    public Button _acceptButton;
    public Button _declineButton;
    public Button _closeButton;
    public TextMeshProUGUI _messageText;

    private bool _confirmDialogActive = false;
    private int _confirmDialogId;

    private void Start()
    {
        Instance = this;

        // Click listeners.
        _acceptButton.onClick.AddListener(AcceptConfirmDialog);
        _declineButton.onClick.AddListener(CloseConfirmDialog);
        _closeButton.onClick.AddListener(CloseConfirmDialog);
        // Close UI.
        CloseConfirmDialog();
    }

    private void Update()
    {
        if (InputManager.ESCAPE_DOWN && _canvas.enabled)
        {
            CloseConfirmDialog();
        }
    }

    public bool IsConfirmDialogActive()
    {
        return _confirmDialogActive;
    }

    public void PlayerConfirm(string question, int dialogId)
    {
        // Return false when waiting other dialog confirm.
        if (_confirmDialogActive)
        {
            return;
        }
        _confirmDialogActive = true;
        _messageText.text = question;
        _confirmDialogId = dialogId;
        _canvas.enabled = true;
    }

    private void AcceptConfirmDialog()
    {
        _canvas.enabled = false;
        switch (_confirmDialogId)
        {
            case 1:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                #endif
                break;

            case 2:
                CharacterSelectionManager.Instance.DeleteCharacter();
                break;

            case 3:
                NetworkManager.SendPacket(new ExitWorldRequest());
                WorldManager.Instance.ExitWorld();
                OptionsManager.Instance.GetOptionsCanvas().enabled = false;
                MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
                break;
        }
        _confirmDialogActive = false;
    }

    private void CloseConfirmDialog()
    {
        _canvas.enabled = false;
        _confirmDialogActive = false;
    }
}
