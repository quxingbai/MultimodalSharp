using MultimodalSharp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;
using static System.Net.WebRequestMethods;

namespace MultimodalSharp.Abstractions.Entities
{
    public delegate void StreamMessageData(String Message, bool IsLastMessage);
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
            await HttpHelper.PostStream(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true), Response,CancelToekn);
        }

        protected virtual async Task<ResponseDataType> GetRequestMessageAsync(RequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.GetData<ResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task<CusteomResponseDataType> GetRequestMessageAsync<CusteomRequestDataType, CusteomResponseDataType>(CusteomRequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.GetData<CusteomResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
    }
}
