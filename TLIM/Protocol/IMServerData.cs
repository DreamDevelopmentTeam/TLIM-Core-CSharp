using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace TLIM.Protocol;

public class IMServerData
{
    public long VersionCode = Versions.VersionCode;
    public string VersionName = Versions.VersionName;
    
    public string ServerName = "TLIM Server";
    public string ServerDescribe = "A TLIM Chat Server.";

    public string ServerPublicKey = "";
    public string ServerPrivateKey = "";

    public List<string> KeyWordList = new List<string>()
    {
        "admin",
        "system",
        "core",
        "console",
        "protocol",
    };
    
    public List<string> SensitiveWordList = new List<string>()
    {
        "刘金玉",
        "刘国强",
        "林爽爽",
        "王可心",
    };
    
    
    // Runtime Data Areas
    
    public List<string> MacBlackList = new List<string>();
    public List<string> IpBlackList = new List<string>();
    public List<Regex> IpBlackListRegex = new List<Regex>();

    public List<int> BannedUserList = new List<int>();
    public List<int> BannedGroupList = new List<int>();
    public List<int> GlobalSilenceUserList = new List<int>();
    
    public Dictionary<int, string> UserNameTable = new Dictionary<int, string>();
    public Dictionary<int, string> UserPasswordTable = new Dictionary<int, string>();
    public Dictionary<int, TcpClient> UserConnectTable = new Dictionary<int, TcpClient>();
    public Dictionary<int, List<int>> GroupUserTable = new Dictionary<int, List<int>>();
    
    






}