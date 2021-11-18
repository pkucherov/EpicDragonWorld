using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Collections;

/**
 * Author: Pantelis Andrianakis
 * Date: September 26th 2018
 */
public class ChatBoxManager : MonoBehaviour
{
    public static ChatBoxManager Instance { get; private set; }

    public GameObject _chatPanel;
    public GameObject _textObject;
    public InputField _inputField;
    private List<Message> _messageList = new List<Message>();
    private static readonly string TIMESTAMP_FORMAT = "HH:mm:ss tt";
    private static readonly int MAX_MESSAGE_COUNT = 50;
    private string _lastTell = "";

    private void Start()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (InputManager.RETURN_DOWN)
        {
            MainManager.Instance.SetChatBoxActive(!MainManager.Instance.IsChatBoxActive());

            if (MainManager.Instance.IsChatBoxActive())
            {
                if (_lastTell.Length > 0)
                {
                    _inputField.text = "/tell " + _lastTell + " ";
                    StartCoroutine(MoveToTextEndOnNextFrame());
                }
                _inputField.ActivateInputField();
                return;
            }

            if (_inputField.text.Length > 0)
            {
                if (Application.isEditor)
                {
                    SendMessageToChat(_inputField.text, 1);
                }
                else
                {
                    NetworkManager.SendPacket(new ChatRequest(_inputField.text));
                    string[] messageSplit = Regex.Replace(_inputField.text, @"\s+", " ").Trim().Split(' ');
                    if (messageSplit.Length > 2 && messageSplit[0].ToLower().Equals("/tell"))
                    {
                        _lastTell = messageSplit[1];
                    }
                    else
                    {
                        _lastTell = "";
                    }
                }
                _inputField.text = "";
                _inputField.DeactivateInputField();
            }
        }

        if (InputManager.ESCAPE_DOWN && MainManager.Instance.IsChatBoxActive())
        {
            MainManager.Instance.SetChatBoxActive(false);
            _inputField.DeactivateInputField();
        }
    }

    private IEnumerator MoveToTextEndOnNextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        _inputField.MoveTextEnd(false); // Do this during the next frame.
    }

    public void SendMessageToChat(string text, int type)
    {
        if (_messageList.Count >= MAX_MESSAGE_COUNT)
        {
            Destroy(_messageList[0].textObject.gameObject);
            _messageList.Remove(_messageList[0]);
        }
        Message message = new Message { text = OptionsManager.Instance.UseChatTimestamps() ? DateTime.Now.ToString(TIMESTAMP_FORMAT) + " " + text : text };
        GameObject newText = Instantiate(_textObject, _chatPanel.transform);
        message.textObject = newText.GetComponent<Text>();
        message.textObject.text = message.text;
        switch (type)
        {
            case 0: // system
                message.textObject.color = Util.IntToColor(OptionsManager.Instance.GetChatColorSystemIntValue());
                break;

            case 1: // normal chat
                message.textObject.color = Util.IntToColor(OptionsManager.Instance.GetChatColorNormalIntValue());
                break;

            case 2: // personal message
                message.textObject.color = Util.IntToColor(OptionsManager.Instance.GetChatColorMessageIntValue());
                break;
        }
        _messageList.Add(message);
    }
}

public class Message
{
    public string text;
    public Text textObject;
}