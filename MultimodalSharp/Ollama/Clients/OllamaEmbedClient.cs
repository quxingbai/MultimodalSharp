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
    public class OllamaEmbedClient : VectorDatabseEmbedingModelBase<OllamaEmbedRequestModel, OllamaEmbedResponseModel>, ITTLEmbedding
    {
        public OllamaEmbedClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/embed")
        {

        }

        public void AddEmbedVectorData(string Text, float[] Embedding)
        {
            base.AddToVectorDatabase(Text, Embedding);
        }

        public IEnumerable<(string Text, float Similarity)> QueryEmbedingText(float[] VectorData, int TopK)
        {
            var data = base.QueryVectorDatabaseTopk(VectorData, TopK);
            return data.Select(x => (x.Text, x.Score));
        }

        public IEnumerable<(string Text, float Similarity)> QueryEmbedingSimilarityText(float[] VectorData, int TopK,float Similarity)
        {
            var data = base.QueryVectorDatabase(VectorData);
            return data.Select(x => (x.Text, x.Score));
        }

        public async Task<float[][]> RequestEmbeddingAsync(params string[] Texts)
        {
            var data = await RequestMessageAsync(new OllamaEmbedRequestModel()
            {
                Model = ModelName,
                Input = Texts
            });
            return data.Embeddings;
        }

        public async Task<float[][]> RequestEmbeddingSaveAsync(params string[] Texts)
        {
            var data = await RequestEmbeddingAsync(Texts);
            for (int i = 0; i < Texts.Length; i++)
            {
                var text = Texts[i];
                var vector= data[i];
                AddEmbedVectorData(text, vector);
            }
            return data;
        }
    }
}
