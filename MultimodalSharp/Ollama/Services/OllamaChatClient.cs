using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Helper;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Ollama.Services
{
    public class OllamaChatClient : TLLSendBaseClient<OllamaChatRequestModel, OllamaChatResponseModel>, ITTLChatCompletion
    {
        public List<OllamaChatMessageModel> ChatMessages = new();
        public OllamaChatClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/chat")
        {
        }



        /// <summary>
        /// 获取上下文历史
        /// </summary>
        public virtual IEnumerable<OllamaChatMessageModel> GetChatMessages()
        {
            return ChatMessages;
        }

        public Task<string> SendMessageAsync(string Message)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageAsync(string Message, Action<string> Response)
        {
            throw new NotImplementedException();
        }
    }
}
