/**
 * Author: Pantelis Andrianakis
 * Date: June 10th 2018
 */
public class Logout
{
    public static void Process(ReceivablePacket packet)
    {
        // Used for kicked message.
        NetworkManager.SetForcedDisconnection(true);
        NetworkManager.DisconnectFromServer();
        // Force exiting to login screen.
        WorldManager.Instance.SetKickFromWorld(true);
    }
}
