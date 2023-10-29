using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TLIM.Protocol.Messages;

[Serializable]
public class ClientHelloMessage : IMessage
{

    public long ver { get; set; }
    public string mac { get; set; }
    public string key { get; set; }
    

    public string ToJsonString()
    {
        string jsonString = JsonSerializer.Serialize(this, ConstValues.JsonOptions);
        return jsonString;
    }

    public static T FromJsonString<T>(string jsonString)
    {
        T newObject = JsonSerializer.Deserialize<T>(jsonString);
        return newObject;
    }
}