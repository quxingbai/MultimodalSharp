using MultimodalSharp.Ollama.Services;
using System.Net;

var client = new OllamaTextClient(new() { HttpClient = new(), ModelName = "deepseek-r1", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
var msg = client.SendMessageAsync("帮我写一个冒泡排序法 C#的", msg =>
{
    Console.Write(msg);
});

while (true)
{
     client.SendMessageAsync(Console.ReadLine(), msg =>
    {
        Console.Write(msg);
    });
}

ollama