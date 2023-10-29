using System.Text.Json;

namespace TLIM;

public class ConstValues
{
    public static readonly bool DebugMode = true;
    public static readonly bool DeepDebugMode = true;
    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions{
        WriteIndented = true
    };
    
    public static readonly int FindServerProtocolPort = 65000;
    
}