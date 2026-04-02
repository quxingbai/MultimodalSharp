using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses.OllamaServiceTagsResponseModel;

namespace MultimodalSharp.Ollama.Models.Entities
{
    public class OllamaResponses
    {

        #region OllamaServices返回类型
        public class OllamaServiceTagsResponseModel
        {
            public class TagModelInfo
            {
                [JsonPropertyName("name")]
                public string Name { get; set; }

                [JsonPropertyName("model")]
                public string Model { get; set; }

                [JsonPropertyName("modified_at")]
                public string ModifiedAt { get; set; }

                [JsonPropertyName("size")]
                public long Size { get; set; }

                [JsonPropertyName("digest")]
                public string Digest { get; set; }

                [JsonPropertyName("details")]
                public ModelDetails Details { get; set; }
            }

            public class ModelDetails
            {
                [JsonPropertyName("parent_model")]
                public string ParentModel { get; set; }

                [JsonPropertyName("format")]
                public string Format { get; set; }

                [JsonPropertyName("family")]
                public string Family { get; set; }

                [JsonPropertyName("families")]
                public List<string> Families { get; set; }

                [JsonPropertyName("parameter_size")]
                public string ParameterSize { get; set; }

                [JsonPropertyName("quantization_level")]
                public string QuantizationLevel { get; set; }
            }

            [JsonPropertyName("models")]
            public List<TagModelInfo> Models { get; set; }
        }
        public class OllamaServiceShowModelResponseModel
        {
            public class ShowModelInfo : Dictionary<string, object>
            {
                // 通用字段（所有模型都有）
                [JsonPropertyName("general.architecture")]
                public string Architecture
                {
                    get => GetValue<string>("general.architecture");
                    set => this["general.architecture"] = value;
                }

                [JsonPropertyName("general.parameter_count")]
                public long? ParameterCount
                {
                    get => GetValue<long?>("general.parameter_count");
                    set => this["general.parameter_count"] = value;
                }

                [JsonPropertyName("general.file_type")]
                public int? FileType
                {
                    get => GetValue<int?>("general.file_type");
                    set => this["general.file_type"] = value;
                }

                [JsonPropertyName("general.quantization_version")]
                public int? QuantizationVersion
                {
                    get => GetValue<int?>("general.quantization_version");
                    set => this["general.quantization_version"] = value;
                }

                [JsonPropertyName("general.size_label")]
                public string SizeLabel
                {
                    get => GetValue<string>("general.size_label");
                    set => this["general.size_label"] = value;
                }

                [JsonPropertyName("general.license")]
                public string License
                {
                    get => GetValue<string>("general.license");
                    set => this["general.license"] = value;
                }

                [JsonPropertyName("general.type")]
                public string ModelType
                {
                    get => GetValue<string>("general.type");
                    set => this["general.type"] = value;
                }

                [JsonPropertyName("general.basename")]
                public string Basename
                {
                    get => GetValue<string>("general.basename");
                    set => this["general.basename"] = value;
                }

                // 便捷方法：获取任意字段
                public T GetValue<T>(string key, T defaultValue = default)
                {
                    if (TryGetValue(key, out var value) && value is T t)
                        return t;

                    // 尝试类型转换
                    try
                    {
                        if (value != null)
                            return (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch { }

                    return defaultValue;
                }

                // 架构特定字段的便捷访问（可选）
                public long? ContextLength
                {
                    get
                    {
                        return Architecture switch
                        {
                            "llama" => GetValue<long?>("llama.context_length"),
                            "qwen2" => GetValue<long?>("qwen2.context_length"),
                            "qwen3" => GetValue<long?>("qwen3.context_length"),
                            "mistral" => GetValue<long?>("mistral.context_length"),
                            _ => null
                        };
                    }
                }

                public int? EmbeddingLength
                {
                    get
                    {
                        return Architecture switch
                        {
                            "llama" => GetValue<int?>("llama.embedding_length"),
                            "qwen2" => GetValue<int?>("qwen2.embedding_length"),
                            "qwen3" => GetValue<int?>("qwen3.embedding_length"),
                            "mistral" => GetValue<int?>("mistral.embedding_length"),
                            _ => null
                        };
                    }
                }

                public int? BlockCount
                {
                    get
                    {
                        return Architecture switch
                        {
                            "llama" => GetValue<int?>("llama.block_count"),
                            "qwen2" => GetValue<int?>("qwen2.block_count"),
                            "qwen3" => GetValue<int?>("qwen3.block_count"),
                            "mistral" => GetValue<int?>("mistral.block_count"),
                            _ => null
                        };
                    }
                }
            }
            [JsonPropertyName("modelfile")]
            public string Modelfile { get; set; }

            [JsonPropertyName("parameters")]
            public string Parameters { get; set; }

            [JsonPropertyName("template")]
            public string Template { get; set; }

            [JsonPropertyName("details")]
            public ModelDetails Details { get; set; }

            [JsonPropertyName("model_info")]
            public ShowModelInfo ModelInfo { get; set; }

            [JsonPropertyName("license")]
            public string License { get; set; }

            [JsonPropertyName("capabilities")]
            public List<string> Capabilities { get; set; }
        }
        #endregion


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
