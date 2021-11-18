/**
 * Author: Pantelis Andrianakis
 * Date: December 26th 2017
 */
public class RecievablePacketHandler
{
    public static void Handle(ReceivablePacket packet)
    {
        switch (packet.ReadShort())
        {
            case 1:
                AccountAuthenticationResult.Process(packet);
                break;

            case 2:
                CharacterSelectionInfoResult.Process(packet);
                break;

            case 3:
                CharacterCreationResult.Process(packet);
                break;

            case 4:
                CharacterDeletionResult.Process(packet);
                break;

            case 5:
                PlayerOptionsInformation.Process(packet);
                break;

            case 6:
                PlayerInformation.Process(packet);
                break;

            case 7:
                NpcInformation.Process(packet);
                break;

            case 8:
                DeleteObject.Process(packet);
                break;

            case 9:
                Logout.Process(packet);
                break;

            case 10:
                LocationUpdate.Process(packet);
                break;

            case 11:
                AnimatorUpdate.Process(packet);
                break;

            case 12:
                ChatResult.Process(packet);
                break;

            case 13:
                PlayerInventoryUpdate.Process(packet);
                break;
        }
    }
}
