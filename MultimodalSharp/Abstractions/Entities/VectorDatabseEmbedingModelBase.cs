using MultimodalSharp.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Abstractions.Entities
{
    public abstract class VectorDatabseEmbedingModelBase<RequestDataType, ResponseDataType> : SendModelBase<RequestDataType, ResponseDataType>
    {
        public record class VectorDatabaseItem(string Text, float[] Vector);
        public record class VectorDatabaseQueryItem(string Text, float Score);
        protected class VectorDatabase
        {
            private List<VectorDatabaseItem> _data = new();

            public void Add(string text, float[] vector)
            {
                _data.Add(new(text, vector));
            }

            public List<VectorDatabaseQueryItem> Query(float[] queryVector, int topK)
            {
                var results = new List<VectorDatabaseQueryItem>();

                foreach (var item in _data)
                {
                    var similarity = CosineSimilarity(queryVector, item.Vector);
                    results.Add(new(item.Text, similarity));
                }

                return results
                    .OrderByDescending(x => x.Score)
                    .Take(topK)
                    .ToList();
            }

            /*
             
                     相似度范围    质量评估
                    0.85-1.00    ✅ 完美匹配，直接可用
                    0.70-0.85    ✅ 相关，有帮助
                    0.60-0.70    ⚠️ 弱相关，可能误导
                    0.50-0.60    ❌ 基本无关，有害
                    < 0.50       ❌ 完全无关，严重污染

             */
            /// <summary>
            /// 根据相似度确定结果
            /// </summary>
            /// <param name="queryVector">向量数据</param>
            /// <param name="similarityThreshold">最小相似度1~0.5</param>
            public List<VectorDatabaseQueryItem> Query(float[] queryVector,int topK, float similarityThreshold = 0.7F)
            {
                var results = new List<VectorDatabaseQueryItem>();
                foreach (var item in _data)
                {
                    var similarity = CosineSimilarity(queryVector, item.Vector);
                    if (similarity >= similarityThreshold)
                    {
                        results.Add(new(item.Text, similarity));
                    }
                }
                return results
                    .OrderByDescending(x => x.Score)
                    .Take(topK)
                    .ToList();
            }
            private float CosineSimilarity(float[] a, float[] b)
            {
                float dotProduct = 0, normA = 0, normB = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    dotProduct += a[i] * b[i];
                    normA += a[i] * a[i];
                    normB += b[i] * b[i];
                }
                return dotProduct / (float)(Math.Sqrt(normA) * Math.Sqrt(normB));
            }
        }

        protected  VectorDatabase _VectorDatabase = new();
        protected VectorDatabseEmbedingModelBase(string ModelName, HttpClient Http, string BaseUrl) : base(ModelName, Http, BaseUrl)
        {
        }

        protected void AddToVectorDatabase(string text, float[] vector)
        {
            _VectorDatabase.Add(text, vector);
        }
        protected IEnumerable<VectorDatabaseQueryItem> QueryVectorDatabaseTopk(float[] queryVector, int topK = 5)
        {
            return _VectorDatabase.Query(queryVector, topK);
        }
        protected IEnumerable<VectorDatabaseQueryItem> QueryVectorDatabase(float[] queryVector,int topK=5, float similarityThreshold = 0.7F)
        {
            return _VectorDatabase.Query(queryVector,topK, similarityThreshold);
        }
    }
}
