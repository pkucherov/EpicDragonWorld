using System.Collections.Generic;

/**
 * Author: Pantelis Andrianakis
 * Date: March 12th 2020
 */
public class PlayerInventoryUpdate
{
    public static void Process(ReceivablePacket packet)
    {
        int itemCount = packet.ReadInt();

        List<ItemHolder> items = new List<ItemHolder>(itemCount);
        for (int i = 0; i < itemCount; i++)
        {
            ItemHolder itemHolder = new ItemHolder(ItemData.GetItemTemplate(packet.ReadInt()));
            itemHolder.SetSlot(packet.ReadInt()); // 1 Head, 2 Chest, 3 Legs, 4 Hands, 5 Feet, 6 Left hand, 7 Right hand, followed by inventory slots.
            itemHolder.SetQuantity(packet.ReadInt());
            itemHolder.SetEnchant(packet.ReadInt());

            items.Add(itemHolder);
        }

        // TODO: Use items to update inventory.
    }
}
