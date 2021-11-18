using System.Collections.Generic;

/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class AnimatorUpdate
{
    public static void Process(ReceivablePacket packet)
    {
        // Read data.
        long objectId = packet.ReadLong();
        float velocityX = packet.ReadFloat();
        float velocityZ = packet.ReadFloat();
        bool triggerJump = packet.ReadByte() == 1;
        bool isInWater = packet.ReadByte() == 1;
        bool isGrounded = packet.ReadByte() == 1;

        ((IDictionary<long, AnimationHolder>)WorldManager.Instance.GetAnimationQueue()).Remove(objectId);
        WorldManager.Instance.GetAnimationQueue().TryAdd(objectId, new AnimationHolder(velocityX, velocityZ, triggerJump, isInWater, isGrounded));
    }
}
