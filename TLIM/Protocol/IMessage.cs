using System.Text.Json.Nodes;

namespace TLIM.Protocol;

public interface IMessage
{
    public string ToJsonString();
    public static T FromJsonString<T>(string jsonString) => throw new NotImplementedException();
}