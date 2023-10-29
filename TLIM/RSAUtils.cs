namespace TLIM;

using System;
using System.Security.Cryptography;
using System.Text;


public class RSAUtils
{
    // Generate an RSA key pair and return the public key in XML format
    /*public static string GenerateRSAKeyPair()
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            return rsa.ToXmlString(true);
        }
    }*/
    
    public static string[] GenerateRSAKeyPair()
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            string publicKey = rsa.ToXmlString(false); // Include only the public key
            string privateKey = rsa.ToXmlString(true); // Include both public and private keys
            return new string[] { publicKey, privateKey };
        }
    }

    // Encrypt data using RSA with the given public key
    public static byte[] EncryptData(string publicKeyXml, byte[] data)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKeyXml);
            return rsa.Encrypt(data, false);
        }
    }

    // Decrypt data using RSA with the given private key
    public static byte[] DecryptData(string privateKeyXml, byte[] encryptedData)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKeyXml);
            return rsa.Decrypt(encryptedData, false);
        }
    }

    // Sign data using RSA with the given private key
    public static byte[] SignData(string privateKeyXml, byte[] data)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKeyXml);
            return rsa.SignData(data, new SHA384CryptoServiceProvider());
        }
    }

    // Verify the signature of data using RSA with the given public key
    public static bool VerifySignature(string publicKeyXml, byte[] data, byte[] signature)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKeyXml);
            return rsa.VerifyData(data, new SHA384CryptoServiceProvider(), signature);
        }
    }
}