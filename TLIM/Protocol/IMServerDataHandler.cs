using System.Text.Json.Nodes;

namespace TLIM.Protocol;

public class IMServerDataHandler
{
    private IMServerData _imServerData;
    
    public IMServerDataHandler(IMServerData imServerData)
    {
        this._imServerData = imServerData;
        
    }    
    
    public bool ProtocolMessageHandler(MCPServerAcceptHandler sah, JsonObject jsonData)
    {
        // Console.WriteLine("C->S: " + jsonData.ToString());
        

        return false;
    }
}