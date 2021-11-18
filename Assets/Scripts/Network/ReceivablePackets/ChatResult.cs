/**
 * Author: Pantelis Andrianakis
 * Date: August 19th 2018
 */
public class ChatResult
{
    public static void Process(ReceivablePacket packet)
    {
        // Read the data.
        int chatType = packet.ReadByte(); // 0 system, 1 normal chat, 2 personal message
        string senderName = packet.ReadString();
        string message = packet.ReadString();

        // Send the message.
        ChatBoxManager.Instance.SendMessageToChat(senderName + ": " + message, chatType);
    }
}
