using MultimodalSharp.Deepseek.Clients;
using MultimodalSharp.Ollama.Clients;
using System.Net;

HttpClient http = new();

//string apiKey = File.ReadAllText("C:\\Users\\BAI\\Desktop\\deepseek_key.txt");
//DeepseekWebV1EmbedClient embedClient = new(http, apiKey);
//await embedClient.RequestEmbeddingSaveAsync("今天天气多云，好像下雨了。","今天太阳不太好","天气预报说注意天气");
//var queryEmbed=await embedClient.RequestEmbeddingAsync("什么天气");
//var query= embedClient.QueryEmbedingText(queryEmbed[0],5);
//foreach (var i in query)
//{
//    Console.WriteLine(i.ToString());
//}

//DeepseekWebV1ChatClient client = new(http, apiKey);
//client.RequestMessageAsync("请介绍一下自己", (text, islast) =>
//{
//    Console.Write(text);
//    if (islast)
//    {
//        Console.WriteLine();
//        Console.WriteLine("最后一行");
//    }
//});
//var text= await client.RequestMessageAsync("请介绍一下自己");
//Console.WriteLine(text);

//chat拥有对话上下文记录
var client = new OllamaChatClient(new() { HttpClient = new(), ModelName = "deepseek-r1", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
while (true)
{
    var read = Console.ReadLine();
    if (read == "1")
    {
        var ctx = await client.GetCompressedContext();
        Console.WriteLine(ctx);
        client.SetCompressContext(ctx);
    }
    else
    {
        Console.WriteLine();
        await client.RequestMessageAsync(read, (msg, lst) =>
                {
                    Console.Write(msg);
                });
        Console.WriteLine();
    }
    //await client.RequestMessageAsync(read, (text, islast) =>
    //{
    //    Console.Write(text);
    //    if (islast)
    //    {
    //        Console.WriteLine();
    //        Console.WriteLine("最后一行");
    //    }
    //});
}

Console.ReadKey();

