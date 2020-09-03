/**
 * Author: Pantelis Andrianakis
 * Date: December 28th 2017
 */
public class CharacterSelectionInfoRequest : SendablePacket
{
    public CharacterSelectionInfoRequest()
    {
        WriteShort(2); // Packet id.
        WriteString(MainManager.Instance.GetAccountName());
    }
}
