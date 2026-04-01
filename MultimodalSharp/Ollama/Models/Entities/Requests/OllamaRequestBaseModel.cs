using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Requests
{
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
}
