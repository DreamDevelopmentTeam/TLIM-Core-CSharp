using System.Text.Json.Nodes;

namespace TLIM.Protocol;

public class IMClientDataHandler
{
    private IMClientData _imClientData;
    
    public IMClientDataHandler(IMClientData imClientData)
    {
        this._imClientData = imClientData;
        
    }    
    
    public bool ProtocolMessageHandler(MCPClientAcceptHandler cah, JsonObject jsonData)
    {
        // Console.WriteLine("S->C: " + jsonData.ToString());
        

        return false;
    }
}