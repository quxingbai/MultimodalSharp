using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Helper;
using MultimodalSharp.Ollama.Models.Entities;
using MultimodalSharp.Ollama.Models.Entities.Requests;
using MultimodalSharp.Ollama.Models.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Services
{
    public class OllamaTextClient : ITTLTextGeneration
    {
        private HttpClient Http { get; set; }
        private string BaseUrl { get; set; }
        private String ModelName { get; set; }
        public OllamaTextClient(OllamaInitDataModel OllamaInitData)
        {
            this.Http = OllamaInitData.HttpClient;
            this.BaseUrl = $"http://{OllamaInitData.ServerIP.Address}:{OllamaInitData.ServerIP.Port}/api/generate";
            this.ModelName = OllamaInitData.ModelName ;
        }

        public async Task<string> SendMessageAsync(string Message)
        {
            var response= await SendMessageAsync(new OllamaGenerateRequestModel()
            {
                Model = ModelName,
                Prompt = Message,
            });
            return response.Response;
        }
        public async Task<OllamaGenerateResponseModel> SendMessageAsync(OllamaGenerateRequestModel RequestData)
        {
            var content = HttpHelper.CreateJsonContent(RequestData,true);
            var response = await Http.PostAsync(BaseUrl, content);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OllamaGenerateResponseModel>(json);
            return data;
        }

        public Task SendMessageAsync(string Message, Action<string> Response)
        {
            throw new NotImplementedException();
        }
    }
}
