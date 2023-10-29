namespace TLIM;

using System;
using System.Security.Cryptography;
using System.Text;


public class RSABase64Utils
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
            // return new string[] { publicKey, privateKey };
            return new string[]
            {
                Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey)), 
                Convert.ToBase64String(Encoding.UTF8.GetBytes(privateKey)),
            };
        }
    }

    // Encrypt data using RSA with the given public key
    public static byte[] EncryptData(string publicKeyXml, byte[] data)
    {
        publicKeyXml = Encoding.UTF8.GetString(Convert.FromBase64String(publicKeyXml));
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKeyXml);
            return rsa.Encrypt(data, false);
        }
    }

    // Decrypt data using RSA with the given private key
    public static byte[] DecryptData(string privateKeyXml, byte[] encryptedData)
    {
        privateKeyXml = Encoding.UTF8.GetString(Convert.FromBase64String(privateKeyXml));
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKeyXml);
            return rsa.Decrypt(encryptedData, false);
        }
    }

    // Sign data using RSA with the given private key
    public static byte[] SignData(string privateKeyXml, byte[] data)
    {
        privateKeyXml = Encoding.UTF8.GetString(Convert.FromBase64String(privateKeyXml));
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKeyXml);
            return rsa.SignData(data, new SHA384CryptoServiceProvider());
        }
    }

    // Verify the signature of data using RSA with the given public key
    public static bool VerifySignature(string publicKeyXml, byte[] data, byte[] signature)
    {
        publicKeyXml = Encoding.UTF8.GetString(Convert.FromBase64String(publicKeyXml));
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKeyXml);
            return rsa.VerifyData(data, new SHA384CryptoServiceProvider(), signature);
        }
    }
}