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
        /// <summary>
        /// 手动添加一个向量数据
        /// </summary>
        public void AddEmbedVectorData(string Text, float[] Embedding)
        {
            base.AddToVectorDatabase(Text, Embedding);
        }

        /// <summary>
        /// 查询向量数据库，返回文本和相似度，结果根据相似度从高到低排序
        /// </summary>
        /// <param name="VectorData">向量数据</param>
        /// <param name="TopK">拿几个</param>
        /// <returns>(文本 , 相似度)</returns>
        public IEnumerable<(string Text, float Similarity)> QueryEmbedingText(float[] VectorData, int TopK)
        {
            var data = base.QueryVectorDatabaseTopk(VectorData, TopK);
            return data.Select(x => (x.Text, x.Score));
        }

        /// <summary>
        /// 根据向量数据库的相似度查询结果，过滤掉相似度低于指定值的结果，返回文本和相似度，结果根据相似度从高到低排序
        /// </summary>
        /// <param name="VectorData">向量数据</param>
        /// <param name="TopK">拿几个</param>
        /// <param name="Similarity">最小相似度 推荐>=0.7</param>
        /// <returns>(文本 , 相似度)</returns>
        public IEnumerable<(string Text, float Similarity)> QueryEmbedingSimilarityText(float[] VectorData, int TopK, float Similarity = 0.7f)
        {
            var data = base.QueryVectorDatabase(VectorData, TopK, Similarity);
            return data.Select(x => (x.Text, x.Score));
        }
        /// <summary>
        /// 根据提供的文本返回对应数量的向量数据，结果根据文本输入的顺序返回。（此结果不会被记录到数据库）
        /// </summary>
        /// <param name="Texts">文本</param>
        /// <returns>根据提供顺序返回</returns>
        public async Task<float[][]> RequestEmbeddingAsync(params string[] Texts)
        {
            var data = await PostRequestMessageAsync(new OllamaEmbedRequestModel()
            {
                Model = ModelName,
                Input = Texts
            }).ConfigureAwait(false);
            return data.Embeddings;
        }

        /// <summary>
        /// 根据提供的文本返回对应数量的向量数据，并把文本和向量数据保存到内置数据库中，结果根据文本输入的顺序返回。后续可以通过文本搜索来找到相似的文本（结果会被记录到数据库）
        /// </summary>
        /// <param name="Texts">文本</param>
        /// <returns>根据提供顺序返回</returns>
        public async Task<float[][]> RequestEmbeddingSaveAsync(params string[] Texts)
        {
            var data = await RequestEmbeddingAsync(Texts).ConfigureAwait(false);
            for (int i = 0; i < Texts.Length; i++)
            {
                var text = Texts[i];
                var vector = data[i];
                AddEmbedVectorData(text, vector);
            }
            return data;
        }
    }
}
