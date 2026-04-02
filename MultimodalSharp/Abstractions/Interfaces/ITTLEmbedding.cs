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
        public Task<float[][]> RequestEmbeddingAsync(params string[] Texts);
    }
}
