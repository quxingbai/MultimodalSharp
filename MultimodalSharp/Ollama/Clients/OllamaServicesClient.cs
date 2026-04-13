using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Helper;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Ollama.Clients
{
    public class OllamaServicesClient : SendModelBase<dynamic, dynamic>
    {
        private UrlHelper _UrlHelper = null;
        public OllamaServicesClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}")
        {
            this._UrlHelper = new(BaseUrl);
        }

        public async Task<OllamaServiceTagsResponseModel> RequestTagsAsync()
        {
            return await base.GetRequestMessageAsync<string, OllamaServiceTagsResponseModel>(null, _UrlHelper.AppendPath("/api/tags")).ConfigureAwait(false);
        }
        public async Task<OllamaServiceShowModelResponseModel> RequestShowAsync(String ModelName)
        {
            return await base.PostRequestMessageAsync<dynamic, OllamaServiceShowModelResponseModel>(new { model = ModelName }, _UrlHelper.AppendPath("/api/show")).ConfigureAwait(false);
        }

    }
}
