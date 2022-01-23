using System.Security.Cryptography;
using System.Text;

namespace AESEncryptDecrypt;

public class AESEncDec
{
    public static string AESEncryption(string input, string key, string AES_IV)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key[..32]);
        #pragma warning disable SYSLIB0021 // Type or member is obsolete
        using var aesAlg = new AesCryptoServiceProvider();
        #pragma warning restore SYSLIB0021 // Type or member is obsolete
        aesAlg.Key = keyBytes;
        aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV[..16]);

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (StreamWriter swEncrypt = new(csEncrypt))
        {
            swEncrypt.Write(input);
        }
        byte[] bytes = msEncrypt.ToArray();
        return ByteArrayToHexString(bytes);
    }

    public static string AESDecryption(string input, string key, string AES_IV)
    {
        byte[] inputBytes = HexStringToByteArray(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key[..32]);
        #pragma warning disable SYSLIB0021 // Type or member is obsolete
        using AesCryptoServiceProvider aesAlg = new();
        #pragma warning restore SYSLIB0021 // Type or member is obsolete
        aesAlg.Key = keyBytes;
        aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV[..16]);

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        using MemoryStream msEncrypt = new(inputBytes);
        using CryptoStream csEncrypt = new(msEncrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srEncrypt = new(csEncrypt);
        return srEncrypt.ReadToEnd();
    }

    /// <summary>
    /// Convert the specified hex string to a byte array
    /// </summary>
    /// <param name="s">hexadecimal string (eg "7F 2C 4A" or "7F2C4A")</param>
    /// <returns>byte array corresponding to hexadecimal string</returns>
    private static byte[] HexStringToByteArray(string s)
    {
        s = s.Replace(" ", "");
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
            buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
        return buffer;
    }

    /// <summary>
    /// Convert a byte array into a formatted hex string
    /// </summary>
    /// <param name="data">byte array</param>
    /// <returns> formatted hexadecimal string</returns>
    private static string ByteArrayToHexString(byte[] data)
    {
        StringBuilder sb = new(data.Length * 3);
        foreach (byte b in data)
        {
            //hexadecimal number
            sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            //16 digits separated by spaces
            //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
        }
        return sb.ToString().ToUpper();
    }
}

