using System.Text;
using System.Text.Json;

namespace TLIM.Protocol.Messages;

[Serializable]
public class KeyTestMessage
{

    public int code { get; set; }

    public string ToJsonString()
    {
        string jsonString = JsonSerializer.Serialize(this, ConstValues.JsonOptions);
        return jsonString;
    }

    public static KeyTestMessage FromJsonString<KeyTestMessage>(string jsonString)
    {
        KeyTestMessage newObject = JsonSerializer.Deserialize<KeyTestMessage>(jsonString);
        return newObject;
    }
    
    
    public byte[] ToJsonStringEncrypted(string publicKey)
    {
        string jsonString = JsonSerializer.Serialize(this, ConstValues.JsonOptions);
        return RSAUtils.EncryptData(publicKey, Encoding.UTF8.GetBytes(jsonString));
    }
    
    public static KeyTestMessage FromJsonStringEncrypted(byte[] data, string privateKey)
    {
        byte[] jsonString = RSAUtils.DecryptData(privateKey, data);        
        KeyTestMessage newObject = JsonSerializer.Deserialize<KeyTestMessage>(Encoding.UTF8.GetString(jsonString));
        return newObject;
    }
}