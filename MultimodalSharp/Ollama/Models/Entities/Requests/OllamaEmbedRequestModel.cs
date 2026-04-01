using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Requests
{
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
}
