/**
 * Author: Pantelis Andrianakis
 * Date: December 31st 2017
 */
public class CharacterDeletionResult
{
    public static void Notify(ReceivablePacket packet)
    {
        CharacterSelectionManager.Instance.SetWaitingServer(false);
    }
}
