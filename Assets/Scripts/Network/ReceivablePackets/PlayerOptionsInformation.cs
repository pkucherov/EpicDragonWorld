using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: May 18th 2018
 */
public class PlayerOptionsInformation
{
    public static void Process(ReceivablePacket packet)
    {
        OptionsManager.Instance.SetChatColorNormalIntValue(packet.ReadInt());
        OptionsManager.Instance.SetChatColorMessageIntValue(packet.ReadInt());
        OptionsManager.Instance.SetChatColorSystemIntValue(packet.ReadInt());
        OptionsManager.Instance.SetUseChatTimestamps(packet.ReadByte() == 1);

        InputManager.SetKeybind(0, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(1, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(2, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(3, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(4, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(5, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(6, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(7, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(8, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(9, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(10, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(11, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(12, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(13, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(14, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(15, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(16, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(17, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(18, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(19, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(20, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(21, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(22, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(23, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(24, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(25, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(26, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(27, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(28, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(29, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(30, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(31, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(32, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(33, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(34, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(35, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(36, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(37, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(38, (KeyCode)packet.ReadShort());
        InputManager.SetKeybind(39, (KeyCode)packet.ReadShort());
    }
}
