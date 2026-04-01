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
        /// 做一个Json
        /// </summary>
        /// <param name="Model">数据源</param>
        /// <param name="RemoveNullValue">是否从Json中删掉为Null的属性</param>
        public static StringContent CreateJsonContent(object Model,bool RemoveNullValue)
        {
            Dictionary<string, object> d = new();
            foreach (var i in Model.GetType().GetProperties())
            {
                var val = i.GetValue(Model);
                if (val!=null)
                {
                    d.Add(i.Name, val);
                }
            }
            string json = System.Text.Json.JsonSerializer.Serialize(d);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
