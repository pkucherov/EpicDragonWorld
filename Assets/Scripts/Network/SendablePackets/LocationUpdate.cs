﻿/**
* @author Pantelis Andrianakis
*/
public class LocationUpdate : SendablePacket
{
    public LocationUpdate(long objectId, float posX, float posY, float posZ, float heading, int animState, bool isWater)
    {
        WriteShort(8); // Packet id.
        WriteLong(objectId);
        WriteDouble(posX); // TODO: WriteFloat
        WriteDouble(posY); // TODO: WriteFloat
        WriteDouble(posZ); // TODO: WriteFloat
        WriteDouble(heading); // TODO: WriteFloat
        WriteShort(animState);
        if(isWater)
        {
            WriteShort(1);
        }
        else
        {
            WriteShort(0);
        }
    }
}
