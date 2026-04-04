using MultimodalSharp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MultimodalSharp.Deepseek.Models.DeepSeekModels;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;
using static System.Net.WebRequestMethods;

namespace MultimodalSharp.Abstractions.Entities
{
    public delegate void StreamMessageData(String Message, bool IsLastMessage);
    /// <summary>
    /// 发送基类 提供了比较通用的发送方法，适用于大多数场景的发送需求，特殊场景可以继承后重写发送方法
    /// </summary>
    /// <typeparam name="RequestDataType">发送类型</typeparam>
    /// <typeparam name="ResponseDataType">返回类型</typeparam>
    public abstract class SendModelBase<RequestDataType, ResponseDataType>
    {
        protected HttpClient Http = null;
        protected string BaseUrl = null;
        protected String ModelName = null;
        public SendModelBase(String ModelName, HttpClient Http, String BaseUrl)
        {
            this.Http = Http;
            this.BaseUrl = BaseUrl;
            this.ModelName = ModelName;
        }

        protected virtual async Task<ResponseDataType> PostRequestMessageAsync(RequestDataType RequestData)
        {
            return await HttpHelper.PostData<ResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task<ResponseDataType> PostRequestMessageAsync(RequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.PostData<ResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task<CusteomResponseDataType> PostRequestMessageAsync<CusteomRequestDataType, CusteomResponseDataType>(CusteomRequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.PostData<CusteomResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task PostRequestMessageStreamAsync(RequestDataType RequestData, Action<ResponseDataType> Response, CancellationToken? CancelToekn = null)
        {
            await HttpHelper.PostStream(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true), Response, CancelToekn);
        }

        protected virtual async Task<ResponseDataType> GetRequestMessageAsync(RequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.GetData<ResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task<CusteomResponseDataType> GetRequestMessageAsync<CusteomRequestDataType, CusteomResponseDataType>(CusteomRequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.GetData<CusteomResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        /// <summary>
        /// 发送一个自定义Request的流返回请求，适用于一些特殊的流式请求场景，比如需要自定义请求头或者请求方法等
        /// </summary>
        public virtual async Task SendReadStreamAsync<ResponseStreamDataType>(HttpClient Http, HttpRequestMessage RequestMessage, Action<ResponseStreamDataType> Response, CancellationToken? CancelToekn = null) => await HttpHelper.SendReadStream(Http, RequestMessage, Response, CancelToekn);

        /// <summary>
        /// 发送一个自定义Request的流返回请求，可以自定义流式返回数据的解析方式
        /// </summary>
        public virtual async Task SendReadStreamAsync<ResponseStreamDataType>(HttpClient Http, HttpRequestMessage RequestMessage, Action<ResponseStreamDataType> Response, CancellationToken? CancelToekn = null, Func<string, ResponseStreamDataType> ModelConverter = null) => await HttpHelper.SendReadStream(Http, RequestMessage, Response, CancelToekn, ModelConverter);


        /// <summary>
        /// 自定义Request 去请求String返回
        /// </summary>
        public virtual async Task<string?> SendReadStringAsync(HttpClient Http, HttpRequestMessage RequestMessage, CancellationToken? CancelToken=null) => await HttpHelper.SendReadString(Http, RequestMessage, CancelToken);


        /// <summary>
        /// 自定义Request 去请求String返回
        /// </summary>
        public virtual async Task<ResponseDataType?> SendReadAsync<ResponseDataType>(HttpClient Http, HttpRequestMessage RequestMessage, CancellationToken? CancelToken = null,Func<string,ResponseDataType> ModelConverter=null) => await HttpHelper.SendRead<ResponseDataType>(Http, RequestMessage, CancelToken,ModelConverter);
    }
}
