/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class CharacterCreationResult
{
    public static void Notify(ReceivablePacket packet)
    {
        CharacterCreationManager.Instance.SetCreationResult(packet.ReadByte());
    }
}
