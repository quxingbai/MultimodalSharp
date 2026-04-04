using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Deepseek.Helpers;
using MultimodalSharp.Deepseek.Models;
using MultimodalSharp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Deepseek.Models.DeepSeekModels;

namespace MultimodalSharp.Deepseek.Clients
{
    public class DeepseekWebV1EmbedClient : VectorDatabseEmbedingModelBase<DeepSeekEmbeddingRequestModel, DeepSeekEmbeddingResponseModel>, ITTLEmbedding
    {
        private string ApiKey = null;
        public DeepseekWebV1EmbedClient(HttpClient Http, string Apikey, string BaseUrl = "https://api.deepseek.com/v1/embeddings", string ModelName = "deepseek-embed") : base(ModelName, Http, BaseUrl)
        {
            throw new("这个API无法使用\n好像官网根本就不支持....问Deepseek它还说支持这个接口，闹麻了...");
            this.ApiKey = Apikey;
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
          

            var msg = CreateRequestMessage(new DeepSeekEmbeddingRequestModel()
            {
                Model = ModelName,
                Input = Texts
            });

            var response = await base.SendReadAsync<DeepSeekEmbeddingResponseModel>(Http, msg);
            return response.Data.Select(s => s.Embedding).ToArray();
        }

        /// <summary>
        /// 根据提供的文本返回对应数量的向量数据，并把文本和向量数据保存到内置数据库中，结果根据文本输入的顺序返回。后续可以通过文本搜索来找到相似的文本（结果会被记录到数据库）
        /// </summary>
        /// <param name="Texts">文本</param>
        /// <returns>根据提供顺序返回</returns>
        public async Task<float[][]> RequestEmbeddingSaveAsync(params string[] Texts)
        {
            var data = await RequestEmbeddingAsync(Texts);
            for (int i = 0; i < Texts.Length; i++)
            {
                var text = Texts[i];
                var vector = data[i];
                AddToVectorDatabase(text, vector);
            }
            return data;
        }

        /// <summary>
        /// 手动添加一个向量数据
        /// </summary>
        public void AddEmbedVectorData(string Text, float[] Embedding)
        {
            base.AddToVectorDatabase(Text, Embedding);
        }

        /// <summary>
        /// 创建默认请求
        /// </summary>
        protected HttpRequestMessage CreateRequestMessage(DeepSeekEmbeddingRequestModel RequestModel) => DeepSeekRequestHelpers.CreateRequestMessage(BaseUrl, ApiKey, HttpMethod.Post, HttpHelper.CreateJsonContent(RequestModel, true));

    }
}
