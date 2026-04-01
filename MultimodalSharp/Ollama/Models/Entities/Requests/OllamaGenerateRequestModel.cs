using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Requests
{
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
}
