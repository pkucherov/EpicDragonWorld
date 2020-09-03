/**
 * Author: Pantelis Andrianakis
 * Date: May 18th 2018
 */
public class PlayerOptionsUpdate : SendablePacket
{
    public PlayerOptionsUpdate()
    {
        WriteShort(12); // Packet id.

        WriteInt(OptionsManager.Instance.GetChatColorNormalIntValue());
        WriteInt(OptionsManager.Instance.GetChatColorMessageIntValue());
        WriteInt(OptionsManager.Instance.GetChatColorSystemIntValue());
        WriteByte(OptionsManager.Instance.UseChatTimestamps() ? 1 : 0);

        WriteShort((int)InputManager.KEY_BINDINGS[0]);
        WriteShort((int)InputManager.KEY_BINDINGS[1]);
        WriteShort((int)InputManager.KEY_BINDINGS[2]);
        WriteShort((int)InputManager.KEY_BINDINGS[3]);
        WriteShort((int)InputManager.KEY_BINDINGS[4]);
        WriteShort((int)InputManager.KEY_BINDINGS[5]);
        WriteShort((int)InputManager.KEY_BINDINGS[6]);
        WriteShort((int)InputManager.KEY_BINDINGS[7]);
        WriteShort((int)InputManager.KEY_BINDINGS[8]);
        WriteShort((int)InputManager.KEY_BINDINGS[9]);
        WriteShort((int)InputManager.KEY_BINDINGS[10]);
        WriteShort((int)InputManager.KEY_BINDINGS[11]);
        WriteShort((int)InputManager.KEY_BINDINGS[12]);
        WriteShort((int)InputManager.KEY_BINDINGS[13]);
        WriteShort((int)InputManager.KEY_BINDINGS[14]);
        WriteShort((int)InputManager.KEY_BINDINGS[15]);
        WriteShort((int)InputManager.KEY_BINDINGS[16]);
        WriteShort((int)InputManager.KEY_BINDINGS[17]);
        WriteShort((int)InputManager.KEY_BINDINGS[18]);
        WriteShort((int)InputManager.KEY_BINDINGS[19]);
        WriteShort((int)InputManager.KEY_BINDINGS[20]);
        WriteShort((int)InputManager.KEY_BINDINGS[21]);
        WriteShort((int)InputManager.KEY_BINDINGS[22]);
        WriteShort((int)InputManager.KEY_BINDINGS[23]);
        WriteShort((int)InputManager.KEY_BINDINGS[24]);
        WriteShort((int)InputManager.KEY_BINDINGS[25]);
        WriteShort((int)InputManager.KEY_BINDINGS[26]);
        WriteShort((int)InputManager.KEY_BINDINGS[27]);
        WriteShort((int)InputManager.KEY_BINDINGS[28]);
        WriteShort((int)InputManager.KEY_BINDINGS[29]);
        WriteShort((int)InputManager.KEY_BINDINGS[30]);
        WriteShort((int)InputManager.KEY_BINDINGS[31]);
        WriteShort((int)InputManager.KEY_BINDINGS[32]);
        WriteShort((int)InputManager.KEY_BINDINGS[33]);
        WriteShort((int)InputManager.KEY_BINDINGS[34]);
        WriteShort((int)InputManager.KEY_BINDINGS[35]);
        WriteShort((int)InputManager.KEY_BINDINGS[36]);
        WriteShort((int)InputManager.KEY_BINDINGS[37]);
        WriteShort((int)InputManager.KEY_BINDINGS[38]);
        WriteShort((int)InputManager.KEY_BINDINGS[39]);
    }
}
