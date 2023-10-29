using System.Net;
using System.Net.Sockets;
using TLIM.Core;

namespace TLIM.Protocol;

public class MCPServer
{
    private int _tcpServerPort;
    private TcpListener _tcpListener;
    private TcpListener _tcpListenerV6;

    private IMServerData _imServerData;

    public MCPServer(IMServerData serverData, int tcpServerPort)
    {
        this._imServerData = serverData;
        this._tcpServerPort = tcpServerPort;
        this._tcpListener = new TcpListener(IPAddress.Any , tcpServerPort);
        this._tcpListenerV6 = new TcpListener(IPAddress.IPv6Any , tcpServerPort);

        
        StartHandlerThreads();
    }
    
    private void StartHandlerThreads()
    {
        Thread IPv4Thread = new Thread(() =>
        {
            PackageHandlerThread(this._tcpListener);
        });
        Thread IPv6Thread = new Thread(() =>
        {
            PackageHandlerThread(this._tcpListenerV6);
        });
        
        IPv4Thread.Start();
        IPv6Thread.Start();
    }

    private void PackageHandlerThread(TcpListener socket)
    {
        while (true)
        {
            try
            {
                TcpClient client = socket.AcceptTcpClient();
                MCPServerAcceptHandler handler = new MCPServerAcceptHandler(client, this._imServerData);
            }
            catch (Exception ex)
            {
                DebugUtils.DebugOut(ex.ToString());
            }
        }
    }
    
    
}