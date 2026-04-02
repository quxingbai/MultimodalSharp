using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Helper;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Ollama.Clients
{
    public class OllamaTextClient : TLLSendBaseClient<OllamaGenerateRequestModel, OllamaGenerateResponseModel>, ITTLTextGeneration
    {
        public OllamaTextClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/generate")
        {
        }
        /// <summary>
        /// 发文本消息 一次性接收所有回复文本
        /// </summary>

        public async Task<string> RequestMessageAsync(string Message)
        {
            var response = await RequestMessageAsync(new OllamaGenerateRequestModel()
            {
                Model = ModelName,
                Prompt = Message,
            });
            return response.Response;
        }

        /// <summary>
        /// 发文本消息 以流式方式接收回复文本
        /// </summary>
        /// <param name="Response">每次收到文本后的回调</param>
        public async Task RequestMessageAsync(string Message, StreamMessageData Response)
        {
            await base.RequestMessageStreamAsync(new OllamaGenerateRequestModel()
            {
                Model = ModelName,
                Prompt = Message,
                Stream = true
            }, data =>
            {
                Response(data.Response, data.Done);
            });
        }

    }
}
