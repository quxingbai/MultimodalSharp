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

        protected virtual async Task<ResponseDataType> RequestMessageAsync(RequestDataType RequestData)
        {
            return await HttpHelper.PostData<ResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task<ResponseDataType> RequestMessageAsync(RequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.PostData<ResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task<CusteomResponseDataType> RequestMessageAsync<CusteomResponseDataType,CusteomRequestDataType>(CusteomRequestDataType RequestData, String BaseUrl)
        {
            return await HttpHelper.PostData<CusteomResponseDataType>(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true));
        }
        protected virtual async Task RequestMessageStreamAsync(RequestDataType RequestData, Action<ResponseDataType> Response)
        {
            await HttpHelper.PostStream(Http, BaseUrl, HttpHelper.CreateJsonContent(RequestData, true), Response);
        }
    }
}
