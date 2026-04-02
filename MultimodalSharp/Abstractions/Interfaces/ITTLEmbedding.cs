using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Abstractions.Interfaces
{
    /// <summary>
    /// 向量嵌入接口
    /// </summary>
    internal interface ITTLEmbedding
    {
        /// <summary>
        /// 请求获取文本对应的向量数据
        /// </summary>
        /// <param name="Texts">文本</param>
        public Task<float[][]> RequestEmbeddingAsync(params string[] Texts);
        /// <summary>
        /// 请求获取文本对应的向量数据并把文本和向量数据添加到数据库里 以便后续查询
        /// </summary>
        /// <param name="Texts">文本</param>
        public Task<float[][]> RequestEmbeddingSaveAsync(params string[] Texts);
        /// <summary>
        /// 把文本和向量数据添加到数据库里 以便后续查询
        /// </summary>
        /// <param name="Text">文本</param>
        /// <param name="VectorData">向量数据</param>
        public void AddEmbedVectorData(string Text, float[] VectorData);
        /// <summary>
        /// 根据相似度拿到前几个文本
        /// </summary>
        /// <param name="VectorData">向量数据</param>
        /// <param name="TopK">拿几个</param>
        /// <returns></returns>
        public IEnumerable<(String Text, float Similarity)> QueryEmbedingText(float[] VectorData, int TopK);


    }
}
