/**
 * AES Rijndael encryption.
 * Author: Pantelis Andrianakis
 * Date: December 23rd 2017
 */
class Encryption
{
    public static byte[] Process(byte[] bytes)
    {
        short keyCounter = 0;
        for (short i = 0; i < bytes.Length; i++)
        {
            bytes[i] ^= EncryptionConfigurations.SECRET_KEYWORD[keyCounter++];
            if (keyCounter == EncryptionConfigurations.SECRET_KEYWORD.Length)
            {
                keyCounter = 0;
            }
        }
        return bytes;
    }
}