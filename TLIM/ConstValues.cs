using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace TLIM;

public class ConstValues
{
    public static readonly bool DebugMode = true;
    public static readonly bool DeepDebugMode = true;
    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions{
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNameCaseInsensitive = true,
        
    };
    
    public static readonly int FindServerProtocolPort = 65000;
    
}