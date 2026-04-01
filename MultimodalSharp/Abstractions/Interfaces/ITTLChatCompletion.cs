using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Abstractions.Interfaces
{
    /// <summary>
    /// 对话补全接口
    /// </summary>
    internal interface ITTLChatCompletion
    {
        public Task<string> SendMessageAsync(string Message);
        public Task SendMessageAsync(string Message, Action<string> Response);

    }
}
