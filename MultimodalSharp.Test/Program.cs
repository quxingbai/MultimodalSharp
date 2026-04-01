using MultimodalSharp.Ollama.Services;
using System.Net;

var client = new OllamaTextClient(new() { HttpClient = new(), ModelName = "deepseek-r1", ServerIP = new IPEndPoint(IPAddress.Loopback, 12344) });
var msg = client.SendMessageAsync("你好，现在什么时间了");
msg.ContinueWith(w =>
{
    Console.WriteLine(msg.Result);
});
while (true)
{
    Console.ReadKey();
}

