using TLIM.Protocol;

namespace TLIM.Core;

public class IMServer
{
    private int _tcpServerPort = 65000;
    private FindServerProtocol _findServerProtocol;
    

    public IMServer(int tcpServerPort)
    {
        this._tcpServerPort = tcpServerPort;
        this._findServerProtocol = new FindServerProtocol(tcpServerPort);
        
        
        
    }
}