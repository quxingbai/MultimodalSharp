using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Requests
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

}
