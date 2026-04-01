using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Requests
{
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
        public new string KeepAlive { get; set; } = "5m";

        public OllamaChatRequestModel()
        {
            Messages = new List<OlllamaChatMessageRequest>();
        }
    }
}
