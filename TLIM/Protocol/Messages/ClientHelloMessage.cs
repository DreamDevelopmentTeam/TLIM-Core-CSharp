using System.Text.Json.Nodes;
using Newtonsoft.Json;
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

    public static ClientHelloMessage FromJsonString<ClientHelloMessage>(string jsonString)
    {
        ClientHelloMessage newObject = JsonSerializer.Deserialize<ClientHelloMessage>(jsonString);
        return newObject;
    }
}