// See https://aka.ms/new-console-template for more information

using System.Net;
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
