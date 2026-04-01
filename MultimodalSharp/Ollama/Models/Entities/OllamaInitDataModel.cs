using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Ollama.Models.Entities
{
    public class OllamaInitDataModel
    {
        public IPEndPoint ServerIP { get; set; }
        public HttpClient HttpClient { get; set; }
        public String ModelName { get; set; }
    }
}
