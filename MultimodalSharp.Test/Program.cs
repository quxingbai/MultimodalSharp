using MultimodalSharp.Ollama.Clients;
using MultimodalSharp.Ollama.Services;
using System.Net;

var client = new OllamaChatClient(new() { HttpClient = new(), ModelName = "deepseek-r1", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
var clientEmbed = new OllamaEmbedClient(new() { HttpClient = new(), ModelName = "Qwen2.5-Coder:latest", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
var services = new OllamaServices(new() { HttpClient = new(), ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
//var msg = client.RequestMessageAsync("帮我写一个冒泡排序法 C#的", (msg, last) =>
//{
//    Console.Write(msg);
//    if (last)
//    {
//        Console.WriteLine("最后一行！");
//    }
//});

//client.SendMessageAsync("帮我写一个冒泡排序法 C#的");

//clientEmbed.RequestEmbeddingSaveAsync("My Computer Is a notbook").ContinueWith(w=> {
//    Console.WriteLine(w.Result);
//});
//clientEmbed.RequestEmbeddingSaveAsync("There is a river in front of my house").ContinueWith(w => {
//});
//clientEmbed.RequestEmbeddingSaveAsync("我家有水果","我家也有香蕉","天上有云").ContinueWith(w => {
//});

//while (true)
//{
//    // client.RequestMessageAsync(Console.ReadLine(), (msg,last) =>
//    //{
//    //    Console.Write(msg);
//    //});

//    string text = Console.ReadLine();
//    var vector= await clientEmbed.RequestEmbeddingAsync(text);
//    var querydata = clientEmbed.QueryEmbedingText(vector[0], 10);
//    Console.WriteLine(querydata);

//}

var data=await services.RequestShowAsync("deepseek-r1");
Console.ReadKey();