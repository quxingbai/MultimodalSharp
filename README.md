## 这是什么

> 方便C#调用各种模型API的集成类库



## Ollama

> 主要是三个 Http 的API
>
> 分别是：generate、chat、embed
>
> 还有个提供查询的service



### Ollama - generate

`/api/generate`

手动设置Context，必要时可以继承重写`GetContext`方法

```c#
//generate不拥有上下文记录，每次请求都是独立的
var generate = new OllamaGenerateClient(new() { HttpClient = http, ModelName = "deepseek-r1", ServerIP = ip }); 
```

提供两种方便方式，全返回和流返回，流方式就是多了个 **委托**（文本，是否最后一句）

```c#

//以全文回复方式请求
async Task GenerateAsync(string message)
{
    var response = await generate.RequestMessageAsync(message);
    Console.WriteLine(response);
}
//以流式回复方式请求
async Task GenerateStreamAsync(string message)
{
    Console.WriteLine("流开始");
    Console.WriteLine();
    await generate.RequestMessageAsync(message, (text, isLast) =>
   {
       Console.Write(text);
       if (isLast)
       {
           Console.WriteLine();
           Console.WriteLine("流结束");
       }
   });
}

```



> 如果希望携带Context
>
> 通过SetContext可以让每次都带着这份Context发消息

想要获取 或者自定义发送方式，有提供重载后的 `RequestMessageAsync`



### Ollama - chat

`/api/chat`

会自在内部动维护一个messages列表，必要时可以继承重写`GetChatMessages`方法。

对外提供一个 `AppendChatMessage` 可以手动增加一些消息内容，有些system消息就可提前写入

```c#
//chat拥有对话上下文记录
var chat = new OllamaChatClient(new() { HttpClient = http, ModelName = "deepseek-r1", ServerIP = ip });
```

使用上和Generate一样

```C#
//以全文回复方式请求
async Task ChatTextAsync(string message)
{
    var response = await chat.RequestMessageAsync(message);
    Console.WriteLine(response);
}
//以流式回复方式请求
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
```



如果想自定义请求，提供重载后的`RequestMessageAsync`方法，不过这些传递Model的都不会自动维护上下文，拿到的数据不会被自动添加到内部的ChatMessages里，所以需要的话得手动扔进去。

GetChatMessage、AppendChatMessage、RemoveChatMessage对外提供三种上下文操作，必要的话可以继承重这三个虚方法





### Ollama - embed

`/api/embd`

继承于 `VectorDatabseEmbedingModelBase` 内部维护了一个 向量数据库 `VectorDatabase`，所以通

```c#
//embed是文本向量化的接口，输入文本，输出向量,内置向量数据库，支持文本搜索。coder模型算的向量对中文好像不太友好。
var embed = new OllamaEmbedClient(new() { HttpClient = http, ModelName = "Qwen2.5-Coder:latest", ServerIP = ip });
```

使用上 可以通过`RequestEmbeddingSaveAsync`拿到并且自动存到内置的**向量数据库**中，之后可以通过`RequestEmbeddingAsync`获取到文本向量 这样就不会放到数据库，然后`QueryEmbedingText`进行相似度匹配，一般来说 **大于0.7**能有点用

```C#

​					  相似度范围    		质量评估
​              	      0.85-1.00    ✅ 完美匹配，直接可用
​                	  0.70-0.85    ✅ 相关，有帮助
​                 	  0.60-0.70    ⚠️ 弱相关，可能误导
​                  	  0.50-0.60    ❌ 基本无关，有害
​                  	  < 0.50       ❌ 完全无关，严重污染

```

```C#
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
```



### Ollama - Services

提供一些 Ollama 的基础api，像 tags show。

```c#
//tags
public async Task<OllamaServiceTagsResponseModel> RequestTagsAsync()

//show
public async Task<OllamaServiceShowModelResponseModel> RequestShowAsync(String ModelName)

```

