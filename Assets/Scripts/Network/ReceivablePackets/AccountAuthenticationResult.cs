/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class AccountAuthenticationResult
{
    public static void Notify(ReceivablePacket packet)
    {
        LoginManager.Instance.SetStatus(packet.ReadByte());
    }
}
