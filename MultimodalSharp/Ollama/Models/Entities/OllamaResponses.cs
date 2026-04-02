using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities
{
    public class OllamaResponses
    {
        public class OllamaResponseBaseModel
        {
            public String HttpStateCode { get; set; }
            public String HttpMessage { get; set; }

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

        public class OllamaChatResponseModel : OllamaResponseBaseModel
        {
            [JsonPropertyName("message")]
            public OllamaChatMessageModel Message { get; set; }

            [JsonPropertyName("load_duration")]
            public long LoadDurationNanoseconds { get; set; }

            [JsonIgnore]
            public TimeSpan LoadDuration => TimeSpan.FromTicks(LoadDurationNanoseconds / 100);

            [JsonPropertyName("prompt_eval_duration")]
            public long PromptEvalDurationNanoseconds { get; set; }

            [JsonIgnore]
            public TimeSpan PromptEvalDuration => TimeSpan.FromTicks(PromptEvalDurationNanoseconds / 100);

            [JsonPropertyName("eval_count")]
            public int EvalCount { get; set; }

            [JsonPropertyName("eval_duration")]
            public long EvalDurationNanoseconds { get; set; }

            [JsonIgnore]
            public TimeSpan EvalDuration => TimeSpan.FromTicks(EvalDurationNanoseconds / 100);

            [JsonPropertyName("done")]
            public bool Done { get; set; }

            [JsonPropertyName("done_reason")]
            public string DoneReason { get; set; }
        }
        public class OllamaEmbedResponseModel : OllamaResponseBaseModel
        {
            [JsonPropertyName("embeddings")]
            public float[][] Embeddings { get; set; }
        }
        public class OllamaGenerateResponseModel : OllamaResponseBaseModel
        {
            [JsonPropertyName("response")]
            public string Response { get; set; }

            [JsonPropertyName("context")]
            public List<int> Context { get; set; }

            [JsonPropertyName("load_duration")]
            public long LoadDurationNanoseconds { get; set; }

            [JsonIgnore]
            public TimeSpan LoadDuration => TimeSpan.FromTicks(LoadDurationNanoseconds / 100);

            [JsonPropertyName("prompt_eval_duration")]
            public long PromptEvalDurationNanoseconds { get; set; }

            [JsonIgnore]
            public TimeSpan PromptEvalDuration => TimeSpan.FromTicks(PromptEvalDurationNanoseconds / 100);

            [JsonPropertyName("eval_count")]
            public int EvalCount { get; set; }

            [JsonPropertyName("eval_duration")]
            public long EvalDurationNanoseconds { get; set; }

            [JsonIgnore]
            public TimeSpan EvalDuration => TimeSpan.FromTicks(EvalDurationNanoseconds / 100);

            [JsonPropertyName("done")]
            public bool Done { get; set; }

            [JsonPropertyName("done_reason")]
            public string DoneReason { get; set; }
        }
        public class OllamaGenerateStreamResponseModel : OllamaResponseBaseModel
        {
            [JsonPropertyName("response")]
            public String Response { get; set; }
            [JsonPropertyName("done")]
            public bool Done { get; set; }
        }
    }
}
