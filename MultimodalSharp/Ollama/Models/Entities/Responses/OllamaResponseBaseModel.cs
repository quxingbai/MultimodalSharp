using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities.Responses
{
    public class OllamaResponseBaseModel
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("total_duration")]
        public long TotalDurationNanoseconds { get; set; }

        [JsonIgnore]
        public TimeSpan TotalDuration => TimeSpan.FromTicks(TotalDurationNanoseconds / 100);

        [JsonPropertyName("prompt_eval_count")]
        public int PromptEvalCount { get; set; }
    }


}
