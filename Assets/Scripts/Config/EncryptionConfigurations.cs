using System.Security.Cryptography;
using System.Text;

/**
 * Author: Pantelis Andrianakis
 * Date: December 25th 2018
 */
public class EncryptionConfigurations
{
    public static readonly byte[] SECRET_KEYWORD = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes("SECRET_KEYWORD"));
}
