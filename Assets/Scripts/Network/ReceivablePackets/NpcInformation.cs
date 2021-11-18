/**
 * Author: Pantelis Andrianakis
 * Date: November 28th 2019
 */
public class NpcInformation
{
    public static void Process(ReceivablePacket packet)
    {
        // Server information.
        long objectId = packet.ReadLong();
        CharacterDataHolder npcTemplate = NpcData.GetNpc(packet.ReadInt());
        CharacterDataHolder characterData = new CharacterDataHolder();
        characterData.SetX(packet.ReadFloat());
        characterData.SetY(packet.ReadFloat());
        characterData.SetZ(packet.ReadFloat());
        characterData.SetHeading(packet.ReadFloat());
        characterData.SetCurrentHp(packet.ReadLong());
        // Client information.
        characterData.SetName(npcTemplate.GetName());
        characterData.SetRace(npcTemplate.GetRace());
        characterData.SetHeight(npcTemplate.GetHeight());
        characterData.SetBelly(npcTemplate.GetBelly()); ;
        characterData.SetHairType(npcTemplate.GetHairType());
        characterData.SetHairColor(npcTemplate.GetHairColor());
        characterData.SetSkinColor(npcTemplate.GetSkinColor());
        characterData.SetEyeColor(npcTemplate.GetEyeColor());
        characterData.SetHeadItem(npcTemplate.GetHeadItem());
        characterData.SetChestItem(npcTemplate.GetChestItem());
        characterData.SetLegsItem(npcTemplate.GetLegsItem());
        characterData.SetHandsItem(npcTemplate.GetHandsItem());
        characterData.SetFeetItem(npcTemplate.GetFeetItem());
        characterData.SetLeftHandItem(npcTemplate.GetLeftHandItem());
        characterData.SetRightHandItem(npcTemplate.GetRightHandItem());
        characterData.SetMaxHp(npcTemplate.GetMaxHp());
        characterData.SetTargetable(npcTemplate.IsTargetable());

        WorldManager.Instance.UpdateObject(objectId, characterData);
    }
}
