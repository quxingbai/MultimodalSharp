using MultimodalSharp.Ollama.Clients;
using System.Net;

var client = new OllamaChatClient(new() { HttpClient = new(), ModelName = "deepseek-r1", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
var clientEmbed = new OllamaEmbedClient(new() { HttpClient = new(), ModelName = "Qwen2.5-Coder:latest", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
//var msg = client.RequestMessageAsync("帮我写一个冒泡排序法 C#的", (msg, last) =>
//{
//    Console.Write(msg);
//    if (last)
//    {
//        Console.WriteLine("最后一行！");
//    }
//});

//client.SendMessageAsync("帮我写一个冒泡排序法 C#的");

clientEmbed.RequestEmbeddingAsync("我的电脑是笔记本电脑机械革命旷世").ContinueWith(w=> {
    Console.WriteLine(w.Result);
});


while (true)
{
    client.RequestMessageAsync(Console.ReadLine(), (msg,last) =>
   {
       Console.Write(msg);
   });
}

