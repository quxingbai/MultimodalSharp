using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MultimodalSharp.Helper.HttpHelper;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Helper
{
    public class HttpHelper
    {
        public delegate void HttpResponseStreamData<ResponseStreamDataType>(ResponseStreamDataType StreamRead);
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
        /// <param name="Model">数据源 null则为空字符串</param>
        /// <param name="RemoveNullValue">是否从Json中删掉为Null的属性</param>
        public static StringContent CreateJsonContent(object? Model, bool RemoveNullValue)
        {
            Dictionary<string, object> d = new();
            string json = "";
            if (Model == null)
            {
            }
            else if (!IsSimpleType(Model.GetType()))
            {
                foreach (var i in Model.GetType().GetProperties())
                {
                    var val = i.GetValue(Model);
                    if (val != null)
                    {
                        d.Add(i.Name, val);
                    }
                }
                json = System.Text.Json.JsonSerializer.Serialize(d);
            }
            else
            {
                json = Model.ToString();
            }
            return new StringContent(json, Encoding.UTF8, "application/json");

            bool IsSimpleType(Type type)
            {
                // 1. 基础类型和常用简单类型
                if (type.IsPrimitive || type.IsEnum) return true;

                // 2. 特殊处理的简单类型
                if (type == typeof(string)) return true;
                if (type == typeof(decimal)) return true;
                if (type == typeof(DateTime)) return true;
                if (type == typeof(DateTimeOffset)) return true;
                if (type == typeof(TimeSpan)) return true;
                if (type == typeof(Guid)) return true;

                // 3. 可空类型（Nullable<T>）
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return IsSimpleType(type.GetGenericArguments()[0]);
                }

                return false;
            }
        }

        /// <summary>
        /// Post数据并等待结果返回 适用于一次性返回的接口
        /// </summary>
        /// <typeparam name="ResponseDataType">返回数据类型</typeparam>
        /// <param name="Url">地址</param>
        /// <param name="Content">携带Content</param>
        /// <returns></returns>
        public static async Task<ResponseDataType> PostData<ResponseDataType>(HttpClient Http, String Url, HttpContent Content)
        {
            var response = await Http.PostAsync(Url, Content);
            var json = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var data = JsonSerializer.Deserialize<ResponseDataType>(json);
            return data;
        }
        /// <summary>
        /// Post数据并以流的形式等待结果返回 适用于流式接口 只要接口一直在返回数据就会一直在Response里返回 直到Response里返回true为止
        /// </summary>
        /// <typeparam name="ResponseStreamDataType">返回流数据类型</typeparam>
        /// <param name="Url">地址</param>
        /// <param name="Content">携带Content</param>
        /// <param name="Response">返回回调 ，如果返回true则不继续读取，否则持续读取目标返回的Stream</param>
        /// <returns></returns>
        public static async Task PostStream<ResponseStreamDataType>(HttpClient Http, String Url, HttpContent Content, Action<ResponseStreamDataType> Response)
        {
            var send = await Http.SendAsync(new HttpRequestMessage(HttpMethod.Post, Url) { Content = Content }, HttpCompletionOption.ResponseHeadersRead);
            send.EnsureSuccessStatusCode();
            var stream = send.Content.ReadAsStream();
            StreamReader reader = new StreamReader(stream);

            while (true)
            {
                var line = reader.ReadLine();
                if (line == null) break;
                var model = JsonSerializer.Deserialize<ResponseStreamDataType>(line);
                Response(model);
            }

        }
    }

}
