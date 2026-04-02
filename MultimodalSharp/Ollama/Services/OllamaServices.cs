using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Helper;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Ollama.Services
{
    public class OllamaServices : SendModelBase<dynamic, dynamic>
    {
        private UrlHelper _UrlHelper = null;
        public OllamaServices(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}")
        {
            this._UrlHelper = new(BaseUrl);
        }

        public async Task<OllamaServiceTagsResponseModel> RequestTagsAsync()
        {
            return await base.RequestMessageAsync<string, OllamaServiceTagsResponseModel>(null, _UrlHelper.AppendPath("/api/tags"));
        }
        public async Task<OllamaServiceShowModelResponseModel> RequestShowAsync(String ModelName)
        {
            return await base.RequestMessageAsync<dynamic, OllamaServiceShowModelResponseModel>(new { model = ModelName }, _UrlHelper.AppendPath("/api/show"));
        }

    }
}
