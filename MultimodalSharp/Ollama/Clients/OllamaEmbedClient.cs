using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Ollama.Clients
{
    public class OllamaEmbedClient : TLLSendBaseClient<OllamaEmbedRequestModel, OllamaEmbedResponseModel>,ITTLEmbedding
    {
        public OllamaEmbedClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/embed")
        {

        }

        public async Task<float[][]> RequestEmbeddingAsync(params string[] Texts)
        {
            var data=await RequestMessageAsync(new OllamaEmbedRequestModel()
            {
                Model = ModelName,
                Input=Texts
            });
            return data.Embeddings;
        }

    }
}
