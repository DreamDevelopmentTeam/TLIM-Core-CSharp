using TLIM.Protocol;

namespace TLIM.Core;

public class IMServer
{
    private int _tcpServerPort = 65000;
    private FindServerProtocol _findServerProtocol;
    private IMServerData _imServerData;
    

    public IMServer(int tcpServerPort, IMServerData serverInfo)
    {
        this._tcpServerPort = tcpServerPort;
        this._findServerProtocol = new FindServerProtocol(tcpServerPort);
        this._imServerData = serverInfo;
        string[] pskp = RSAUtils.GenerateRSAKeyPair();
        this._imServerData.ServerPublicKey = pskp[0];
        this._imServerData.ServerPrivateKey = pskp[1];

    }
}