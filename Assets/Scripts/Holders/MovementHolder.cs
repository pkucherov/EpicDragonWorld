/**
 * Author: Pantelis Andrianakis
 * Date: April 21st 2019
 */
public class MovementHolder
{
    public readonly float _posX;
    public readonly float _posY;
    public readonly float _posZ;
    public readonly float _heading;

    public MovementHolder(float posX, float posY, float posZ, float heading)
    {
        _posX = posX;
        _posY = posY;
        _posZ = posZ;
        _heading = heading;
    }

    public float GetX()
    {
        return _posX;
    }

    public float GetY()
    {
        return _posY;
    }

    public float GetZ()
    {
        return _posZ;
    }

    public float GetHeading()
    {
        return _heading;
    }
}
