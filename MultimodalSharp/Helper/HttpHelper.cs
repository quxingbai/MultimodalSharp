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
            //Dictionary<string, object> d = new();
            string json = "";
            if (Model == null)
            {
            }
            else if (!IsSimpleType(Model.GetType()))
            {
                if (RemoveNullValue)
                {
                    json = System.Text.Json.JsonSerializer.Serialize(Model, new JsonSerializerOptions()
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    });
                }
                else
                {
                    json = JsonSerializer.Serialize(Model);
                }
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
        public static async Task<ResponseDataType> GetData<ResponseDataType>(HttpClient Http, String Url, HttpContent Content)
        {
            var response = await Http.GetAsync(Url);
            var json = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var data = JsonSerializer.Deserialize<ResponseDataType>(json);
            return data;
        }
        /// <summary>
        /// Post数据并以流的形式等待结果返回 适用于流式接口
        /// </summary>
        /// <typeparam name="ResponseStreamDataType">返回流数据类型</typeparam>
        /// <param name="Url">地址</param>
        /// <param name="Content">携带Content</param>
        /// <param name="Response">返回回调 ，如果返回true则不继续读取，否则持续读取目标返回的Stream</param>
        /// <returns></returns>
        public static async Task PostStream<ResponseStreamDataType>(HttpClient Http, String Url, HttpContent Content, Action<ResponseStreamDataType> Response, CancellationToken? CancelToken = null) => await SendReadStream(Http, new HttpRequestMessage(HttpMethod.Post, Url) { Content = Content }, Response, CancelToken);

        /// <summary>
        /// 发送一个自定义Request的流返回请求
        /// </summary>
        /// <param name="Http">Http</param>
        /// <param name="RequestMessage">消息</param>
        /// <param name="Response">如果是流就一行一行读取返回，如果是整条就直接返回结果</param>
        /// <param name="Option">ResponseHeadersRead表示行的方式 流读取</param>
        public static async Task SendRead(HttpClient Http, HttpRequestMessage RequestMessage, Action<string> Response, HttpCompletionOption Option = HttpCompletionOption.ResponseHeadersRead, CancellationToken? CancelToken = null)
        {
            var send = await Http.SendAsync(RequestMessage, Option);
            CancelToken?.ThrowIfCancellationRequested();
            send.EnsureSuccessStatusCode();
            //如果是流式接口则以流的形式读取返回结果
            if (Option == HttpCompletionOption.ResponseHeadersRead)
            {
                await Task.Run(() =>
                {
                    var stream = send.Content.ReadAsStream();
                    StreamReader reader = new StreamReader(stream);
                    while (true)
                    {
                        CancelToken?.ThrowIfCancellationRequested();
                        var line = reader.ReadLine();
                        if (line == null) break;
                        if (line == "") continue;
                        Response(line);
                    }
                });
            }
            else
            {
                var text = await send.Content.ReadAsStringAsync();
                CancelToken?.ThrowIfCancellationRequested();
                Response(text);
            }
        }
        /// <summary>
        /// 自定义消息读取一条String
        /// </summary>
        public static async Task<string> SendReadString(HttpClient Http, HttpRequestMessage RequestMessage, CancellationToken? CancelToken = null)
        {
            string result = null;
            await SendRead(Http, RequestMessage, (string line) =>
            {
                result = line;
            }, HttpCompletionOption.ResponseContentRead, CancelToken);
            return result;
        }
        /// <summary>
        /// 自定义Http消息读取一条String并转换为Model 适用于一次性返回的接口
        /// </summary>
        /// <typeparam name="ResponseModelType"返回类型></typeparam>
        /// <param name="ModelConverter">转换方式，如果为Null就用序列化方式</param>
        /// <returns></returns>
        public static async Task<ResponseModelType?> SendRead<ResponseModelType>(HttpClient Http, HttpRequestMessage RequestMessage, CancellationToken? CancelToken = null, Func<string, ResponseModelType>? ModelConverter = null)
        {
            var text = await SendReadString(Http, RequestMessage, CancelToken);
            return ModelConverter == null ? JsonSerializer.Deserialize<ResponseModelType>(text) : ModelConverter(text);
        }
        /// <summary>
        /// 自定义流式接口的模型转换器 适用于流式接口 
        /// </summary>
        public static async Task SendReadStream(HttpClient Http, HttpRequestMessage RequestMessage, Action<string> Response, CancellationToken? CancelToken = null)
        {
            await SendRead(Http, RequestMessage, Response, HttpCompletionOption.ResponseHeadersRead, CancelToken);
        }
        /// <summary>
        /// 自定义流式接口的模型转换器 
        /// </summary>
        public static async Task SendReadStream<ResponseStreamDataType>(HttpClient Http, HttpRequestMessage RequestMessage, Action<ResponseStreamDataType> Response, CancellationToken? CancelToken = null)
        {
            await SendReadStream(Http, RequestMessage, (string line) =>
            {
                var data = JsonSerializer.Deserialize<ResponseStreamDataType>(line);
                if (data != null)
                {
                    Response(data);
                }
            }, CancelToken);
        }
        /// <summary>
        /// 自定义流式接口的模型转换器 适用于流式接口 
        /// </summary>
        /// <typeparam name="ResponseStreamDataType">返回类型</typeparam>
        /// <param name="Http">Http</param>
        /// <param name="RequestMessage">请求消息</param>
        /// <param name="Response">返回数据</param>
        /// <param name="CancelToken">取消</param>
        /// <param name="ModelConverter">从文本消息转换为Model的手动方式，不提供就用Json的默认序列化</param>
        /// <returns></returns>
        public static async Task SendReadStream<ResponseStreamDataType>(HttpClient Http, HttpRequestMessage RequestMessage, Action<ResponseStreamDataType> Response, CancellationToken? CancelToken = null, Func<string, ResponseStreamDataType> ModelConverter = null)
        {
            await SendReadStream(Http, RequestMessage, (string line) =>
            {
                var data = ModelConverter == null ? JsonSerializer.Deserialize<ResponseStreamDataType>(line) : ModelConverter(line);
                if (data != null)
                {
                    Response(data);
                }
            }, CancelToken);
        }
    }

}
