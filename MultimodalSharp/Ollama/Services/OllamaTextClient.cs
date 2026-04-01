using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Ollama.Models.Entities;
using MultimodalSharp.Ollama.Models.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Services
{
    public class OllamaTextClient:ITTLTextGeneration
    {
        private HttpClient Http { get; set; }
        private string BaseUrl { get; set; }
        public OllamaTextClient(OllamaInitDataModel OllamaInitData)
        {
            this.Http = OllamaInitData.HttpClient;
            this.BaseUrl = $"http://{OllamaInitData.ServerIP.Address}:{OllamaInitData.ServerIP.Port}/api/generate";
        }

        public Task<string> SendMessageAsync(string Message)
        {
        }
        public Task<OllamaTextGenerateRequestModel> SendMessageAsync(OllamaTextGenerateRequestModel RequestData)
        {
            Http.PostAsync(BaseUrl,new HttpContent())
        }

        public Task SendMessageAsync(string Message, Action<string> Response)
        {
            throw new NotImplementedException();
        }
    }
}
