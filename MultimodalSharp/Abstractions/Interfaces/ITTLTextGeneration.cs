using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Abstractions.Interfaces
{
    /// <summary>
    /// 文本生成接口
    /// </summary>
    internal interface ITTLTextGeneration
    {
        public Task<string> SendMessageAsync(string Message);
        public Task SendMessageAsync(string Message, Action<string> Response);
    }
}
