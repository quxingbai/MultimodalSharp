using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Helper
{
    public class HttpHelper
    {
        public static readonly HttpClient Http = null;
        static HttpHelper()
        {
            Http = new HttpClient();
            Http.Timeout = TimeSpan.FromSeconds(100);
        }
        public static HttpClient GetHttpClient()
        {
            return Http;
        }
        /// <summary>
        /// 创建一个JsonContent
        /// </summary>
        public static StringContent CreateJsonContent(object Model)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(Model);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
