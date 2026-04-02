using MultimodalSharp.Ollama.Clients;
using System.Net;

HttpClient http = new();
IPEndPoint ip = new IPEndPoint(IPAddress.Loopback, 12344);

//generation不拥有上下文记录，每次请求都是独立的
var generation = new OllamaGenerationClient(new() { HttpClient = http, ModelName = "deepseek-r1", ServerIP = ip });

//chat拥有对话上下文记录
var chat = new OllamaChatClient(new() { HttpClient = http, ModelName = "deepseek-r1", ServerIP = ip });

//embedding是文本向量化的接口，输入文本，输出向量,内置向量数据库，支持文本搜索。coder模型不太能理解中文
var embed = new OllamaEmbedClient(new() { HttpClient = http, ModelName = "Qwen2.5-Coder:latest", ServerIP = ip });

//services是模型管理接口，例如查看模型列表
var services = new OllamaServicesClient(new() { HttpClient = http, ServerIP = ip });

GenerationStreamAsync("你好");
Console.ReadKey();


async Task GenerationAsync(string message)
{
    var response = await generation.RequestMessageAsync(message);
    Console.WriteLine(response);
}
async Task GenerationStreamAsync(string message)
{
    Console.WriteLine("流开始");
    Console.WriteLine();
    await generation.RequestMessageAsync(message, (text, isLast) =>
   {
       Console.Write(text);
       if (isLast)
       {
           Console.WriteLine();
           Console.WriteLine("流结束");
       }
   });
}



async Task ChatTextAsync(string message)
{
    var response = await chat.RequestMessageAsync(message);
    Console.WriteLine(response);
}
async Task ChatStreamAsync(string message)
{
    Console.WriteLine("流开始");
    Console.WriteLine();
    await chat.RequestMessageAsync(message, (text, isLast) =>
   {
       Console.Write(text);
       if (isLast)
       {
           Console.WriteLine();
           Console.WriteLine("流结束");
       }
   });
}



async Task EmbedTest()
{
    //这个方法能把输入文本转换成向量并保存在内置数据库中，后续可以通过文本搜索来找到相似的文本
    await embed.RequestEmbeddingSaveAsync("My computer has a gray 17 inch screen and a 1TB solid-state drive", "My mouse is white and the keyboard is black");

    string message = "Is my computer a solid-state drive";
    var vector = await embed.RequestEmbeddingAsync(message);
    var vectorTexts = embed.QueryEmbedingText(vector[0], 5);
    foreach (var i in vectorTexts)
    {
        //0~1 之间，至少0.7才有意义，越接近1越相似
        Console.WriteLine(i.Similarity + "\t" + i.Text);
    }

}

async Task ServicesTest()
{
    var response = await services.RequestTagsAsync();
    foreach (var model in response.Models)
    {
        Console.WriteLine("模型："+model.Name);
    }
}
