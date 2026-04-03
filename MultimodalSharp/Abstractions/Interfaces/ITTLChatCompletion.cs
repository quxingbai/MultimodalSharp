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
    public interface ITTLChatCompletion<ChatMessageType>
    {
        /// <summary>
        /// 发送并等待结果所有一起返回
        /// </summary>
        public Task<string> RequestMessageAsync(string Message);
        /// <summary>
        /// 发送并返回 但以流的形式
        /// </summary>
        /// <param name="Message">发送</param>
        /// <param name="Response">流读出数据就会在这里返回</param>
        /// <returns></returns
        public Task RequestMessageAsync(string Message, StreamMessageData Response, CancellationToken? CancelToekn = null);
        /// <summary>
        /// 获取上下文历史
        /// </summary>
        public IEnumerable<ChatMessageType> GetChatMessages();
        /// <summary>
        /// 追加上下文
        /// </summary>
        public IEnumerable<ChatMessageType> AppendChatMessage(params ChatMessageType[] Msg);
        /// <summary>
        /// 初始化上下文
        /// </summary>
        public void InitChatMessages(params ChatMessageType[] Msg);
    }
}
