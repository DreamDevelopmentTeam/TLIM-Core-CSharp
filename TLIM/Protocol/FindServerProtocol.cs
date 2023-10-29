using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;

namespace TLIM.Protocol;

public class FindServerProtocol
{
    private Socket _socket;
    private Socket _socketV6;

    private int _serverPort;

    public FindServerProtocol(int tcpServerPort)
    {
        this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this._socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        this._socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        
        this._socketV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        this._socketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        this._socketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        this._socket.Bind(new IPEndPoint(IPAddress.Any, ConstValues.FindServerProtocolPort));
        this._socketV6.Bind(new IPEndPoint(IPAddress.IPv6Any, ConstValues.FindServerProtocolPort));
        
        this._serverPort = tcpServerPort;

        this.StartHandlerThreads();
    }

    private void StartHandlerThreads()
    {
        Thread IPv4Thread = new Thread(() =>
        {
            PackageHandlerThread(this._socket);
        });
        Thread IPv6Thread = new Thread(() =>
        {
            PackageHandlerThread(this._socketV6);
        });
        
        IPv4Thread.Start();
        IPv6Thread.Start();
    }

    private void PackageHandlerThread(Socket socket)
    {
        while (true)
        {
            try
            {
                EndPoint remote;
                if (socket.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remote = new IPEndPoint(IPAddress.IPv6Any, 0);
                }
                else
                {
                    remote = new IPEndPoint(IPAddress.Any, 0);
                }
                byte[] buffer = new byte[1024];
                int len = socket.ReceiveFrom(buffer, ref remote);
                string message = Encoding.UTF8.GetString(buffer);
                DebugUtils.DebugOut("FSP.GetMSG(" + remote.AddressFamily.ToString() + ") : " + message);
                if (message.StartsWith("#TLIM@FindServer%Find"))
                {
                    SendReturnPackage(this._serverPort, (IPEndPoint)remote);
                }
            }
            catch (Exception ex)
            {
                DebugUtils.DebugOut(ex.ToString());
            }
        }
    }

    public void SendReturnPackage(int localServerTcpPort, IPEndPoint client)
    {
        if (client.AddressFamily == AddressFamily.InterNetwork)
        {
            DebugUtils.DebugOut("FSP.SRT(" + client.AddressFamily.ToString() + ") : " + localServerTcpPort);
            byte[] data = Encoding.UTF8.GetBytes("#TLIM@FindServer%Return&" + localServerTcpPort.ToString());
            this._socket.SendTo(data, client);
        }
        else
        {
            DebugUtils.DebugOut("FSP.SRT(" + client.AddressFamily.ToString() + ") : " + localServerTcpPort);
            byte[] data = Encoding.UTF8.GetBytes("#TLIM@FindServer%Return&" + localServerTcpPort.ToString());
            this._socketV6.SendTo(data, client);
        }
    }


    public static List<IPEndPoint> FindServer(int timeout = 3000, int findServerProtocolPort = 0)
    {
        int fcpp = findServerProtocolPort;
        if (fcpp == 0)
        {
            fcpp = ConstValues.FindServerProtocolPort;
        }

        List<IPEndPoint> list = new List<IPEndPoint>();
        
        Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        tempSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        tempSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        byte[] data = Encoding.UTF8.GetBytes("#TLIM@FindServer%Find");
        tempSocket.SendTo(data, (EndPoint)new IPEndPoint(IPAddress.Broadcast, fcpp));
        
        // In one LAN, may have many Servers
        // Get Server ReturnPackage Timeout: 3s
        tempSocket.ReceiveTimeout = timeout;

        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                int receivedBytes = tempSocket.ReceiveFrom(buffer, ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(buffer, 0, receivedBytes);

                if (!message.StartsWith("#TLIM@FindServer%Return"))
                {
                    break;
                }

                int serverPort = int.Parse(message.Split('&')[1]);
                if (serverPort <= 0 || serverPort > 65535)
                {
                    break;
                }

                list.Add(new IPEndPoint(((IPEndPoint)remoteEndPoint).Address, serverPort));
            }
        }
        catch (SocketException ex)
        {
            if (ex.SocketErrorCode == SocketError.TimedOut)
            {
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            DebugUtils.DebugOut(ex.ToString());
        }
    
        return list;
        
        
        
        
    }
    
}