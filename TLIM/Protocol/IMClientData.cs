namespace TLIM.Protocol;

public class IMClientData
{
    public long VersionCode = Versions.VersionCode;
    public string VersionName = Versions.VersionName;
    
    public string UserName = "User";
    public int UID = 10000;
    
    public string ClientPublicKey = "";
    public string ClientPrivateKey = "";
}