using MultimodalSharp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Deepseek.Models.DeepSeekModels;

namespace MultimodalSharp.Deepseek.Helpers
{
    public static class DeepSeekRequestHelpers
    {
        /// <summary>
        /// 创建一个默认请求
        /// </summary>
        public static HttpRequestMessage CreateRequestMessage(String BaseUrl,string ApiKey,HttpMethod Method,HttpContent Content)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content= Content
            };
            req.Headers.Add("Authorization", $"Bearer {ApiKey}");
            return req;
        }
    }
}
