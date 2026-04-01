using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Services
{
    public class OllamaChatClient : ITTLChatCompletion
    {
        private HttpClient Http { get; set; }
        private string BaseUrl { get; set; }
        private String ModelName { get; set; }
        private List<OllamaChatMessageModel> ChatMessages = new();
        public OllamaChatClient(OllamaInitDataModel OllamaInitData)
        {
            this.Http = OllamaInitData.HttpClient;
            this.BaseUrl = $"http://{OllamaInitData.ServerIP.Address}:{OllamaInitData.ServerIP.Port}/api/chat";
            this.ModelName = OllamaInitData.ModelName;
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
