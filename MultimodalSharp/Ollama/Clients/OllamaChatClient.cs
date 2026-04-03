using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Helper;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultimodalSharp.Ollama.Models.Entities.OllamaRequests;
using static MultimodalSharp.Ollama.Models.Entities.OllamaResponses;

namespace MultimodalSharp.Ollama.Clients
{
    public class OllamaChatClient : SendModelBase<OllamaChatRequestModel, OllamaChatResponseModel>, ITTLChatCompletion
    {
        protected List<OlllamaChatRoleMessage> ChatMessages = new();
        public OllamaChatClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/chat")
        {
        }


        /// <summary>
        /// 发文本消息 一次性接收所有回复文本，自动维护上下文历史
        /// </summary>
        public async Task<string> RequestMessageAsync(string Message)
        {
            var data = await RequestMessageAsync(new OllamaChatRequestModel()
            {
                Model = ModelName,
                Messages = AppendChatMessage(OlllamaChatRoleMessage.CreateUserMessage(Message))
            });
            var message = new OlllamaChatRoleMessage()
            {
                Role = data.Message.Role,
                Content = data.Message.Content
            };
            AppendChatMessage(message);
            return message.Content;
        }
        /// <summary>
        /// 发文本消息，通过回调实时接收回复文本，自动维护上下文历史
        /// </summary>
        public async Task RequestMessageAsync(string Message, StreamMessageData Response)
        {
            StringBuilder messageStringB = new();
            string? role = null;
            await RequestMessageAsync(new OllamaChatRequestModel()
            {
                Model = ModelName,
                Stream = true,
                Messages = AppendChatMessage(OlllamaChatRoleMessage.CreateUserMessage(Message))
            }, (data) =>
            {
                var msg = data.Message.Content;
                if (role == null) role = data.Message.Role;
                messageStringB.Append(msg);
                Response(msg, data.Done);
            });

            AppendChatMessage(new() { Role = role, Content = messageStringB.ToString() });
        }

        /// <summary>
        /// 发模型消息 一次性接收所有回复文本。需要手动维护上下文
        /// </summary>
        public async Task<OllamaChatResponseModel> RequestMessageAsync(OllamaChatRequestModel RequestModel) => await PostRequestMessageAsync(RequestModel);

        /// <summary>
        /// 发送模型消息 流式接收回复文本。需要手动维护上下文
        /// </summary>
        public async Task RequestMessageAsync(OllamaChatRequestModel RequestModel, Action<OllamaChatResponseModel> Response) => await PostRequestMessageStreamAsync(RequestModel, Response);


        /// <summary>
        /// 获取上下文历史
        /// </summary>
        public virtual IEnumerable<OlllamaChatRoleMessage> GetChatMessages()
        {
            return ChatMessages;
        }
        /// <summary>
        /// 添加到末尾一条上下文消息
        /// </summary>
        public virtual IEnumerable<OlllamaChatRoleMessage> AppendChatMessage(OlllamaChatRoleMessage Msg)
        {
            ChatMessages.Add(Msg);
            return ChatMessages;
        }
        /// <summary>
        /// 删除一条上下文消息
        /// </summary>
        public virtual bool RemoveChatMessages(OlllamaChatRoleMessage MessageItem)
        {
            return ChatMessages.Remove(MessageItem);
        }

    }
}
