// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text.Json.Nodes;
using TLIM.Protocol;

Console.WriteLine("Hello, World!");

FindServerProtocol fcpServer = new FindServerProtocol(11451);
FindServerProtocol fcpServer2 = new FindServerProtocol(11452);
FindServerProtocol fcpServer3 = new FindServerProtocol(11453);

List<IPEndPoint> findTemp = FindServerProtocol.FindServer();
Console.WriteLine("Server Count : " + findTemp.Count.ToString());
foreach (var ip in findTemp)
{
    Console.WriteLine("{ " + ip.Address.ToString() + " : " + ip.Port.ToString() + " }");
}

IMServerData imServerData = new IMServerData();
MCPServer mcpServer = new MCPServer(imServerData, 11451);

IMClientData imClientData = new IMClientData();
MCPClient client = new MCPClient(imClientData, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11451));

client.SendMessage(
        JsonObject.Parse("{\"test\": 114514}").AsObject()
    );