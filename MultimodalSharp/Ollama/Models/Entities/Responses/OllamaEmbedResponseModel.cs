using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Responses
{
    public class OllamaEmbedResponseModel : OllamaResponseBaseModel
    {
        [JsonPropertyName("embeddings")]
        public List<List<float>> Embeddings { get; set; }
    }

}
