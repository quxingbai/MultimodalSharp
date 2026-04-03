using Microsoft.Extensions.AI;
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
using msAI = Microsoft.Extensions.AI;

namespace MultimodalSharp.Ollama.Clients
{
    public class OllamaChatClient : ChatMessageSendModelBase<OllamaChatRequestModel, OllamaChatResponseModel, OlllamaChatRoleMessage>, ITTLChatCompletion<OlllamaChatRoleMessage>
    {
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
        public async Task RequestMessageAsync(string Message, StreamMessageData Response, CancellationToken? CancelToekn = null)
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
            }, CancelToekn);

            AppendChatMessage(new OlllamaChatRoleMessage() { Role = role, Content = messageStringB.ToString() });
        }

        /// <summary>
        /// 发模型消息 一次性接收所有回复文本。需要手动维护上下文
        /// </summary>
        public async Task<OllamaChatResponseModel> RequestMessageAsync(OllamaChatRequestModel RequestModel) => await PostRequestMessageAsync(RequestModel);

        /// <summary>
        /// 发送模型消息 流式接收回复文本。需要手动维护上下文
        /// </summary>
        public async Task RequestMessageAsync(OllamaChatRequestModel RequestModel, Action<OllamaChatResponseModel> Response, CancellationToken? CancelToekn = null) => await PostRequestMessageStreamAsync(RequestModel, Response, CancelToekn);


        public new virtual IEnumerable<OlllamaChatRoleMessage> GetChatMessages()
        {
            return base.GetChatContextMessages();
        }
        public virtual IEnumerable<OlllamaChatRoleMessage> AppendChatMessage(params OlllamaChatRoleMessage[] Msgs)=>base.AppendChatContextMessage(Msgs);
        public new virtual bool RemoveChatContextMessages(OlllamaChatRoleMessage MessageItem)=>base.RemoveChatContextMessages(MessageItem);
        public void InitChatMessages(params OlllamaChatRoleMessage[] Msgs)=>base.InitChatContextMessages(Msgs);
    }
}
