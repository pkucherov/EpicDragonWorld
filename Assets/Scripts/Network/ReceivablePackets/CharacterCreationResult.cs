/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class CharacterCreationResult
{
    public static void Process(ReceivablePacket packet)
    {
        CharacterCreationManager.Instance.SetCreationResult(packet.ReadByte());
    }
}
