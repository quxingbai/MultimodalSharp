using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities
{
    public class OllamaRequests
    {
        /// <summary>
        /// 对话消息模型
        /// </summary>
        public class OlllamaChatMessageRequest
        {
            [JsonPropertyName("role")]
            public string Role { get; set; }

            [JsonPropertyName("content")]
            public string Content { get; set; }

            [JsonPropertyName("images")]
            public List<string> Images { get; set; }
        }

        /// <summary>
        /// 基础请求模型（包含通用选项）
        /// </summary>
        public abstract class OllamaRequestBaseModel
        {
            [JsonPropertyName("model")]
            public string Model { get; set; }

            [JsonPropertyName("stream")]
            public bool Stream { get; set; } = false;

            [JsonPropertyName("options")]
            public Dictionary<string, object> Options { get; set; }

            [JsonPropertyName("keep_alive")]
            public string KeepAlive { get; set; } = "5m";

            protected OllamaRequestBaseModel()
            {
                Options = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// 文本生成请求模型
        /// </summary>
        public class OllamaGenerateRequestModel : OllamaRequestBaseModel
        {
            [JsonPropertyName("prompt")]
            public string Prompt { get; set; }

            [JsonPropertyName("system")]
            public string System { get; set; }

            [JsonPropertyName("context")]
            public List<int> Context { get; set; }

            [JsonPropertyName("template")]
            public string Template { get; set; }

            [JsonPropertyName("raw")]
            public bool Raw { get; set; }

            [JsonPropertyName("format")]
            public object Format { get; set; }

            [JsonPropertyName("images")]
            public List<string> Images { get; set; }
        }

        /// <summary>
        /// 向量嵌入请求模型
        /// </summary>
        public class OllamaEmbedRequestModel
        {
            [JsonPropertyName("model")]
            public string Model { get; set; }

            [JsonPropertyName("input")]
            public object Input { get; set; }  // 可以是 string 或 List<string>

            [JsonPropertyName("truncate")]
            public bool? Truncate { get; set; }

            [JsonPropertyName("options")]
            public Dictionary<string, object> Options { get; set; }

            [JsonPropertyName("keep_alive")]
            public string KeepAlive { get; set; } = "5m";

            public OllamaEmbedRequestModel()
            {
                Options = new Dictionary<string, object>();
            }
        }


        /// <summary>
        /// 对话请求模型
        /// </summary>
        public class OllamaChatRequestModel : OllamaRequestBaseModel
        {
            [JsonPropertyName("messages")]
            public List<OlllamaChatMessageRequest> Messages { get; set; }

            [JsonPropertyName("tools")]
            public List<object> Tools { get; set; }

            [JsonPropertyName("format")]
            public object Format { get; set; }

            [JsonPropertyName("keep_alive")]
            //public new string KeepAlive { get; set; } = "5m";
            public new string KeepAlive { get; set; } 

            public OllamaChatRequestModel()
            {
                Messages = new List<OlllamaChatMessageRequest>();
            }
        }
    }
}
