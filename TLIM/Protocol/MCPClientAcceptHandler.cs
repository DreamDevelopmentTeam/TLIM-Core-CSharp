using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using TLIM.Protocol.Messages;

namespace TLIM.Protocol;

public class MCPClientAcceptHandler
{
    private TcpClient _client;
    private IMClientData _imClientData;
    private int packageCount = 0;
    
    private bool helloReceived = false;
    private bool testReceived = false;
    private bool authReceived = false;
    private bool encryptionEnabled = false;
    
    private Random ran = new Random();
    private int keyTestCode;

    private ServerHelloMessage serverInfo;

    public MCPClientAcceptHandler(TcpClient client, IMClientData imClientData)
    {
        this._client = client;
        this._imClientData = imClientData;

        this.keyTestCode = ran.Next(0, 33554432);

        StartThreads();
    }

    private void StartThreads()
    {
        Thread thread = new Thread(() =>
        {
            ClientHelloMessage clientHelloMessage = new ClientHelloMessage();
            clientHelloMessage.ver = this._imClientData.VersionCode;
            clientHelloMessage.mac = MacUtils.GetMachineCodePlus();
            clientHelloMessage.key = this._imClientData.ClientPublicKey;
            this._client.Client.Send(Encoding.UTF8.GetBytes(clientHelloMessage.ToJsonString()));
            
            DebugUtils.DeepDebugOut("Server.SAH.Thread : Send Data : " + clientHelloMessage.ToJsonString());
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
                
                DebugUtils.DeepDebugOut("Client.SAH.Thread(" + ip.Address.ToString() + ") : Get Data : " + len.ToString());
                
                string temp = Encoding.UTF8.GetString(buffer);
                
                if (!this.helloReceived && this.packageCount == 0)
                {
                    // New Connect
                    this.serverInfo = ServerHelloMessage.FromJsonString<ServerHelloMessage>(temp);

                    KeyTestMessage keyTestMessage = new KeyTestMessage();
                    keyTestMessage.code = this.keyTestCode;
                    
                    socket.Send(keyTestMessage.ToJsonStringEncrypted(this.serverInfo.key));
                    this.helloReceived = true;
                    continue;
                }
                if (!this.testReceived && this.packageCount == 1)
                {
                    KeyTestMessage keyTestMessage = KeyTestMessage.FromJsonStringEncrypted(buffer, this._imClientData.ClientPrivateKey);
                    DebugUtils.DebugOut("Client.SAH.Thread(" + ip.Address.ToString() + ") : Key Test Code = " + keyTestMessage.code.ToString());

                    if (keyTestMessage.code != this.keyTestCode)
                    {
                        this._client.Close();
                        DebugUtils.DebugOut("Client.SAH.Thread(" + ip.Address.ToString() + ") : Key Test Code -> Close");
                        return;
                    }
                    
                    this.testReceived = true;
                    this.encryptionEnabled = true;
                    continue;
                }
                
                
                string jsonString = Encoding.UTF8.GetString(
                        RSAUtils.DecryptData(this._imClientData.ClientPrivateKey, buffer)
                    );
                try
                {
                    JsonObject jsonObject = JsonObject.Parse(jsonString).AsObject();
                    if (ProtocolMessageHandler(jsonObject))
                    {
                        this._client.Close();
                        DebugUtils.DebugOut("Client.SAH.Thread(" + ip.Address.ToString() + ") : Message Handler -> Close");
                        return;
                    }
                }catch (Exception ex)
                {
                    try
                    {
                        DebugUtils.DebugOut("Client.SAH.Thread(" + this._client.Client.RemoteEndPoint.ToString() +
                                            ") : Data Exception : \n" + ex.ToString());
                    }
                    catch (Exception iex)
                    {
                    
                    }
                    
                }




            }
            catch (Exception ex)
            {
                try
                {
                    this._client.Close();
                    DebugUtils.DebugOut("Client.SAH.Thread(" + this._client.Client.RemoteEndPoint.ToString() +
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



    public bool ProtocolMessageHandler(JsonObject jsonData)
    {
        Console.WriteLine("S->C: " + jsonData.ToString());


        return false;
    }


    public bool SendMessage(JsonObject jsonData)
    {
        if (!this.encryptionEnabled)
        {
            return false;
        }

        try
        {
            this._client.Client.Send(
                RSAUtils.EncryptData(
                    this.serverInfo.key,
                    Encoding.UTF8.GetBytes(
                        jsonData.ToString()
                    )
                )
            );
            return true;
        }
        catch (Exception ex)
        {
            DebugUtils.DebugOut("Client.SAH.Send(" + this._client.Client.RemoteEndPoint.ToString() +
                                ") : \n" + ex.ToString());
            return false;
        }

        return false;
    }
}