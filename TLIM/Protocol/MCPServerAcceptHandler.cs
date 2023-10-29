using System.Net;
using System.Net.Sockets;
using System.Text;
using TLIM.Core;
using TLIM.Protocol.Messages;

namespace TLIM.Protocol;

public class MCPServerAcceptHandler
{
    private TcpClient _client;
    private IMServerData _imServerData;
    private int packageCount = 0;
    
    private bool helloReceived = false;
    private bool testReceived = false;
    private bool authReceived = false;
    private bool encryptionEnabled = false;
    
    

    private ClientHelloMessage clientInfo;

    public MCPServerAcceptHandler(TcpClient client, IMServerData imServerData)
    {
        this._client = client;
        this._imServerData = imServerData;

        StartThreads();
    }

    private void StartThreads()
    {
        Thread thread = new Thread(() =>
        {
            DataHandlerThread(this._client.Client);
        });
        
        thread.Start();
    }

    private void DataHandlerThread(Socket socket)
    {
        while (true)
        {
            try
            {
                EndPoint endPoint;
                if (socket.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    endPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                }
                else
                {
                    endPoint = new IPEndPoint(IPAddress.Any, 0);
                }
            
                byte[] buffer = new byte[65535];
                int len = socket.ReceiveFrom(buffer, ref endPoint);
                IPEndPoint ip = (IPEndPoint)endPoint;

                if (this._imServerData.IpBlackList.Contains(ip.Address.ToString()))
                {
                    this._client.Close();
                    DebugUtils.DebugOut("Server.SAH.Thread(" + ip.Address.ToString() + ") : IP BlackListed");
                    return;
                }
                if (this._imServerData.IpBlackListRegex.Count != 0)
                {
                    foreach (var regex in this._imServerData.IpBlackListRegex)
                    {
                        if (regex.IsMatch(ip.Address.ToString()))
                        {
                            this._client.Close();
                            DebugUtils.DebugOut("Server.SAH.Thread(" + ip.Address.ToString() + ") : IP Regex BlackListed");
                            return;
                        }
                    }
                }
                
                string temp = Encoding.UTF8.GetString(buffer);
                
                if (!this.helloReceived && this.packageCount == 0)
                {
                    // New Connect
                    this.clientInfo = ClientHelloMessage.FromJsonString<ClientHelloMessage>(temp);
                    
                    if (this._imServerData.MacBlackList.Contains(this.clientInfo.mac))
                    {
                        this._client.Close();
                        DebugUtils.DebugOut("Server.SAH.Thread(" + ip.Address.ToString() + ") : Mac BlackListed");
                        return;
                    }
                    
                    ServerHelloMessage serverHelloMessage = new ServerHelloMessage();
                    serverHelloMessage.ver = this._imServerData.VersionCode;
                    serverHelloMessage.name = this._imServerData.ServerName;
                    serverHelloMessage.desc = this._imServerData.ServerDescribe;
                    serverHelloMessage.key = this._imServerData.ServerPublicKey;
                    socket.Send(Encoding.UTF8.GetBytes(serverHelloMessage.ToJsonString()));
                    this.helloReceived = true;
                    continue;
                }

                
            }
            catch (Exception ex)
            {
                try
                {
                    this._client.Close();
                    DebugUtils.DebugOut("Server.SAH.Thread(" + this._client.Client.RemoteEndPoint.ToString() +
                                        ") : \n" + ex.ToString());
                }
                catch (Exception iex)
                {
                    
                }

                return;
            }

            this.packageCount++;
        }
    }
    
}