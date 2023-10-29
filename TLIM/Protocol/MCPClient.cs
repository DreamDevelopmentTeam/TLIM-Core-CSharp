using System.Net;
using System.Net.Sockets;
using System.Text.Json.Nodes;

namespace TLIM.Protocol;

public class MCPClient
{

    private TcpClient _tcpClient;
    private IMClientData _imClientData;

    private MCPClientAcceptHandler mcpClientAcceptHandler;
    private IPEndPoint serverAddress;
    public MCPClient(IMClientData clientData, IPEndPoint remoteAddress)
    {
        this._imClientData = clientData;
        this.serverAddress = remoteAddress;
        this._tcpClient = new TcpClient(remoteAddress);
        
        StartHandlerThreads();
    }
    
    private void StartHandlerThreads()
    {
        Thread thread = new Thread(() =>
        {
            this._tcpClient.Connect(this.serverAddress);
            this.mcpClientAcceptHandler =
                new MCPClientAcceptHandler(this._tcpClient, this._imClientData);
        });
        
        thread.Start();
    }

    public bool SendMessage(JsonObject jsonData)
    {
        return this.mcpClientAcceptHandler.SendMessage(jsonData);
    }
    
}