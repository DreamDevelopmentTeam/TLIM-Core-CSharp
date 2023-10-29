using TLIM.Protocol;

namespace TLIM.Core;

public class IMClient
{
    private int _tcpServerPort = 65000;
    private IMClientData _imClientData;
    

    public IMClient(int tcpServerPort, IMClientData clientInfo)
    {
        this._tcpServerPort = tcpServerPort;
        this._imClientData = clientInfo;
        string[] pskp = RSAUtils.GenerateRSAKeyPair();
        this._imClientData.ClientPublicKey = pskp[0];
        this._imClientData.ClientPrivateKey = pskp[1];

    }
}