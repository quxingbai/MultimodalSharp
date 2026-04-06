using Microsoft.VisualBasic;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Abstractions.Entities
{
    /// <summary>
    /// 拥有上下文管理的发送基类，通常用于聊天模型，提供上下文历史的维护方法
    /// </summary>
    /// <typeparam name="RequestDataType">发送类型</typeparam>
    /// <typeparam name="ResponseDataType">返回类型</typeparam>
    /// <typeparam name="ChatMessageType">上下文类型</typeparam>
    public abstract class ChatMessageSendModelBase<RequestDataType, ResponseDataType, ChatMessageType> : SendModelBase<RequestDataType, ResponseDataType>
    {
        private List<ChatMessageType> ChatMessages = new();
        public ChatMessageSendModelBase(string ModelName, HttpClient Http, string BaseUrl) : base(ModelName, Http, BaseUrl)
        {
        }


        /// <summary>
        /// 获取上下文历史
        /// </summary>
        protected virtual IEnumerable<ChatMessageType> GetChatContextMessages()
        {
            return ChatMessages;
        }
        /// <summary>
        /// 添加到末尾一条上下文消息，会保存到上下文
        /// </summary>
        protected virtual IEnumerable<ChatMessageType> AppendChatContextMessages(params ChatMessageType[] Msgs)
        {
            foreach (var i in Msgs)
            {
                ChatMessages.Add(i);
            }
            return ChatMessages;
        }
        /// <summary>
        /// 添加到末尾一条上下文消息 但不保存在上下文
        /// </summary>
        protected virtual IEnumerable<ChatMessageType> AppendChatContextMessagesNoSave(params ChatMessageType[] Msgs)
        {
            List<ChatMessageType> msgs = new();
            msgs.AddRange(ChatMessages);
            msgs.AddRange(Msgs);
            return msgs;
        }


        /// <summary>
        /// 添加到末尾一条上下文消息，可选择保存
        /// </summary>
        /// <param name="UserMessage">用户消息</param>
        /// <param name="SystemMessage">系统消息</param>
        /// <param name="SaveUserMessage">是否保存到上自动下文维护</param>
        /// <param name="SaveSystemMessage">是否把这条系统消息也保存到上下文</param>
        /// <returns></returns>
        protected virtual IEnumerable<ChatMessageType> AppendChatContextMessageCanSave(ChatMessageType? UserMessage, ChatMessageType? SystemMessage, bool SaveUserMessage = false, bool SaveSystemMessage = false)
        {
            if (SaveUserMessage && UserMessage != null) AppendChatContextMessages(UserMessage);
            if (SaveSystemMessage && SystemMessage != null) AppendChatContextMessages(SystemMessage);
            List<ChatMessageType> msgs = new();
            msgs.AddRange(ChatMessages);
            if (UserMessage != null) msgs.Add(UserMessage);
            if (SystemMessage != null) msgs.Add(SystemMessage);
            return msgs;
        }
        /// <summary>
        /// 删除一条上下文消息
        /// </summary>
        protected virtual bool RemoveChatContextMessages(ChatMessageType MessageItem)
        {
            return ChatMessages.Remove(MessageItem);
        }

        /// <summary>
        /// 删除一条上下文消息
        /// </summary>
        protected virtual void RemoveAtChatContextMessage(int Index)
        {
            ChatMessages.RemoveAt(Index);
        }

        /// <summary>
        /// 删除最后几条上下文消息
        /// </summary>
        protected virtual int RemoveLastChatContextMessage(int Count)
        {
            int count = Count;
            int remove = 0;
            while (count-- > 0)
            {
                if (ChatMessages.Count == 0) break;
                ChatMessages.RemoveAt(ChatMessages.Count - 1);
                remove++;
            }
            return remove;
        }
        /// <summary>
        /// 清理掉所有上下文消息
        /// </summary>
        protected virtual void ClearChatContextMessages() => ChatMessages.Clear();
        protected virtual void InitChatContextMessages(params ChatMessageType[] Msgs)
        {
            ClearChatContextMessages();
            AppendChatContextMessages(Msgs);
        }
    }
}
