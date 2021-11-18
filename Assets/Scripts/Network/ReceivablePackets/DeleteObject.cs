/**
 * Author: Pantelis Andrianakis
 * Date: June 10th 2018
 */
public class DeleteObject
{
    public static void Process(ReceivablePacket packet)
    {
        long objectId = packet.ReadLong();
        WorldManager.Instance.DeleteObject(objectId);
    }
}
