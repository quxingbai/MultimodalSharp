using MultimodalSharp.Deepseek.Clients;
using MultimodalSharp.Ollama.Clients;
using System.Net;

HttpClient http = new();

string apiKey = File.ReadAllText("C:\\Users\\BAI\\Desktop\\deepseek_key.txt");
DeepseekWebV1ChatClient client = new(http, apiKey);
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


while (true)
{
    var read = Console.ReadLine();
    await client.RequestMessageAsync(read, (text, islast) =>
    {
        Console.Write(text);
        if (islast)
        {
            Console.WriteLine();
            Console.WriteLine("最后一行");
        }
    });
}

Console.ReadKey();

