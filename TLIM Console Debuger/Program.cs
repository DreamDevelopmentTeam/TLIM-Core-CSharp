// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text.Json.Nodes;
using Org.BouncyCastle.Asn1.Cms;
using TLIM;
using TLIM.Protocol;
using TLIM.Protocol.Messages;

Console.WriteLine("Hello, World!");

/*
JsonObject jo = JsonObject.Parse(
    """
    {
      "ver": 10000,
      "mac": "34F435B95DBDA94BC12579FCFEA5194062D05338E0ADB3B5FE645920F4F104A1",
      "key": "\u003CRSAKeyValue\u003E\u003CModulus\u003Ex\u002B70W6ZjqG0T44VSmbILll\u002BmQMrrTWtVoHPpMR4PBvcRRgrUW2amkgZI9zhdgWp54wRCRSYkjO6fnhIKjSVHsF/zO1LARDGYQVZHTHXbGTydSiHmqPhvzg3X/GYEpSEYkVhIGArY7Xn2DtL1waImzN2KFKeOtpdg4TtSiSG\u002BaK0=\u003C/Modulus\u003E\u003CExponent\u003EAQAB\u003C/Exponent\u003E\u003C/RSAKeyValue\u003E"
    }

    """).AsObject();
Console.WriteLine(jo.Count);
Console.WriteLine(jo.ToString());
ClientHelloMessage chm = ClientHelloMessage.FromJsonString<ClientHelloMessage>(jo.ToString());
Console.WriteLine(chm.mac);

return;*/

/*string[] keys = RSAUtils.GenerateRSAKeyPair();
Console.WriteLine(keys[0]);
Console.WriteLine(keys[1]);
return;*/

FindServerProtocol fcpServer = new FindServerProtocol(11451);
FindServerProtocol fcpServer2 = new FindServerProtocol(11452);
FindServerProtocol fcpServer3 = new FindServerProtocol(11453);

/*List<IPEndPoint> findTemp = FindServerProtocol.FindServer();
Console.WriteLine("Server Count : " + findTemp.Count.ToString());
foreach (var ip in findTemp)
{
    Console.WriteLine("{ " + ip.Address.ToString() + " : " + ip.Port.ToString() + " }");
}

return;*/

IMServerData imServerData = new IMServerData();
string[] pskp = RSAUtils.GenerateRSAKeyPair();
imServerData.ServerPublicKey = pskp[0];
imServerData.ServerPrivateKey = pskp[1];
MCPServer mcpServer = new MCPServer(imServerData, 11452);

IMClientData imClientData = new IMClientData();
string[] pskp2 = RSAUtils.GenerateRSAKeyPair();
imClientData.ClientPublicKey = pskp2[0];
imClientData.ClientPrivateKey = pskp2[1];

MCPClient client = new MCPClient(imClientData, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11452));

/*for (int i = 0; i < 10; i++)
{
    Thread.Sleep(200);
    
    Console.WriteLine(
        client.SendMessage(
            JsonObject.Parse("{\"test\": 114514}").AsObject()
        )
    );
}*/