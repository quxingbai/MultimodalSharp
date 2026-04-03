using MultimodalSharp.Ollama.Clients;
using System.Net;

HttpClient http = new();
IPEndPoint ip = new IPEndPoint(IPAddress.Loopback, 12344);

//generate不拥有上下文记录，每次请求都是独立的
var generate = new OllamaGenerateClient(new() { HttpClient = http, ModelName = "deepseek-r1", ServerIP = ip });

//chat拥有对话上下文记录
var chat = new OllamaChatClient(new() { HttpClient = http, ModelName = "deepseek-r1", ServerIP = ip });

//embedding是文本向量化的接口，输入文本，输出向量,内置向量数据库，支持文本搜索。coder模型不太能理解中文
var embed = new OllamaEmbedClient(new() { HttpClient = http, ModelName = "Qwen2.5-Coder:latest", ServerIP = ip });

//services是模型管理接口，例如查看模型列表
var services = new OllamaServicesClient(new() { HttpClient = http, ServerIP = ip });


Console.ReadKey();

