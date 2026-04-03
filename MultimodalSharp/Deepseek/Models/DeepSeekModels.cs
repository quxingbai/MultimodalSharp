using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Deepseek.Models
{
    public class DeepSeekModels
    {
        /// <summary>
        /// DeepSeek 对话请求模型
        /// </summary>
        public class DeepSeekChatRequestModel
        {
            [JsonPropertyName("model")]
            public string Model { get; set; }  // "deepseek-chat" 或 "deepseek-reasoner"

            [JsonPropertyName("messages")]
            public IEnumerable<DeepSeekChatMessage> Messages { get; set; }

            [JsonPropertyName("stream")]
            public bool Stream { get; set; }  // 是否流式输出

            [JsonPropertyName("temperature")]
            public double? Temperature { get; set; }  // 温度参数，0-2之间

            [JsonPropertyName("max_tokens")]
            public int? MaxTokens { get; set; }  // 最大输出token数

            [JsonPropertyName("top_p")]
            public double? TopP { get; set; }

            [JsonPropertyName("frequency_penalty")]
            public double? FrequencyPenalty { get; set; }  // -2 到 2

            [JsonPropertyName("presence_penalty")]
            public double? PresencePenalty { get; set; }  // -2 到 2

            [JsonPropertyName("tools")]
            public List<DeepSeekTool> Tools { get; set; }

            [JsonPropertyName("tool_choice")]
            public object ToolChoice { get; set; }  // "auto", "none", 或特定工具

            [JsonPropertyName("response_format")]
            public DeepSeekResponseFormat ResponseFormat { get; set; }  // JSON模式等

            [JsonPropertyName("stop")]
            public object Stop { get; set; }  // string 或 List<string>
        }

        /// <summary>
        /// DeepSeek 对话消息模型
        /// </summary>
        public class DeepSeekChatMessage
        {
            [JsonPropertyName("role")]
            public string Role { get; set; }  // "system", "user", "assistant", "tool"

            [JsonPropertyName("content")]
            public string Content { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }  // 可选，工具调用时的名称

            [JsonPropertyName("tool_calls")]
            public List<DeepSeekToolCall> ToolCalls { get; set; }  // assistant消息中的工具调用

            [JsonPropertyName("tool_call_id")]
            public string ToolCallId { get; set; }  // tool消息中的调用ID
        }

        /// <summary>
        /// DeepSeek 响应格式模型
        /// </summary>
        public class DeepSeekResponseFormat
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }  // "text" 或 "json_object"
        }

        /// <summary>
        /// DeepSeek 工具定义模型
        /// </summary>
        public class DeepSeekTool
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }  // "function"

            [JsonPropertyName("function")]
            public DeepSeekFunction Function { get; set; }
        }

        /// <summary>
        /// DeepSeek 函数定义模型
        /// </summary>
        public class DeepSeekFunction
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("parameters")]
            public object Parameters { get; set; }  // JSON Schema对象
        }

        /// <summary>
        /// DeepSeek 工具调用模型
        /// </summary>
        public class DeepSeekToolCall
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }  // "function"

            [JsonPropertyName("function")]
            public DeepSeekToolCallFunction Function { get; set; }
        }

        /// <summary>
        /// DeepSeek 工具调用函数模型
        /// </summary>
        public class DeepSeekToolCallFunction
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("arguments")]
            public string Arguments { get; set; }  // JSON字符串
        }

        /// <summary>
        /// DeepSeek 对话响应模型
        /// </summary>
        public class DeepSeekChatResponseModel
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("object")]
            public string Object { get; set; }  // "chat.completion"

            [JsonPropertyName("created")]
            public long Created { get; set; }  // Unix时间戳

            [JsonPropertyName("model")]
            public string Model { get; set; }

            [JsonPropertyName("choices")]
            public List<DeepSeekChoice> Choices { get; set; }

            [JsonPropertyName("usage")]
            public DeepSeekUsage Usage { get; set; }

            [JsonPropertyName("system_fingerprint")]
            public string SystemFingerprint { get; set; }
        }

        /// <summary>
        /// DeepSeek 选择项模型
        /// </summary>
        public class DeepSeekChoice
        {
            [JsonPropertyName("index")]
            public int Index { get; set; }

            [JsonPropertyName("message")]
            public DeepSeekChatMessage Message { get; set; }

            [JsonPropertyName("finish_reason")]
            public string FinishReason { get; set; }  // "stop", "length", "tool_calls"等

            [JsonPropertyName("delta")]
            public DeepSeekChatMessage Delta { get; set; }  // 流式响应中使用
        }

        /// <summary>
        /// DeepSeek Token使用统计模型
        /// </summary>
        public class DeepSeekUsage
        {
            [JsonPropertyName("prompt_tokens")]
            public int PromptTokens { get; set; }

            [JsonPropertyName("completion_tokens")]
            public int CompletionTokens { get; set; }

            [JsonPropertyName("total_tokens")]
            public int TotalTokens { get; set; }
        }

        /// <summary>
        /// DeepSeek 流式响应块模型
        /// </summary>
        public class DeepSeekStreamChunk
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("object")]
            public string Object { get; set; }  // "chat.completion.chunk"

            [JsonPropertyName("created")]
            public long Created { get; set; }

            [JsonPropertyName("model")]
            public string Model { get; set; }

            [JsonPropertyName("choices")]
            public List<DeepSeekChoice> Choices { get; set; }
        }
    }
}
