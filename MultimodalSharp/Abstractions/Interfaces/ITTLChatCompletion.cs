using MultimodalSharp.Abstractions.Entities;
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
        public Task<string> RequestMessageAsync(string Message);
        public Task RequestMessageAsync(string Message, StreamMessageData Response);

    }
}
