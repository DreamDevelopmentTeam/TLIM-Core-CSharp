using System.Net;
using System.Net.Sockets;

namespace TLIM.Protocol;

public class MessageConsoleProtocolServer
{
    private int _tcpServerPort;
    private TcpListener _tcpListener;
    private TcpListener _tcpListenerV6;

    public MessageConsoleProtocolServer(int tcpServerPort)
    {
        this._tcpServerPort = tcpServerPort;
        this._tcpListener = new TcpListener(IPAddress.Any , tcpServerPort);
        this._tcpListenerV6 = new TcpListener(IPAddress.IPv6Any , tcpServerPort);
        
    }
    
    
}