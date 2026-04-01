using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Responses
{
    public class OllamaGenerateStreamResponseModel :OllamaResponseBaseModel
    {
        [JsonPropertyName("response")]
        public String Response { get; set; }
        [JsonPropertyName("done")]
        public bool Done { get; set; }
    }

}
