using System.Text.Json;
using System.Text.Json.Nodes;

namespace TLIM.Protocol;

public interface IMessage
{
    public string ToJsonString();
    public static T FromJsonString<T>(string jsonString)
    {
        T newObject = JsonSerializer.Deserialize<T>(jsonString);
        return newObject;
    }
}