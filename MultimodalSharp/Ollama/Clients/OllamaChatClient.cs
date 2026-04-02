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
        public List<OlllamaChatRoleMessage> ChatMessages = new();
        public OllamaChatClient(OllamaInitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/chat")
        {
        }


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

        public async Task RequestMessageAsync(string Message, StreamMessageData Response)
        {
            StringBuilder messageStringB = new();
            string? role = null;
            await RequestMessageStreamAsync(new OllamaChatRequestModel()
            {
                Model = ModelName,
                Stream = true,
                Messages = AppendChatMessage(OlllamaChatRoleMessage.CreateUserMessage(Message))
            }, (data) =>
            {
                var msg = data.Message.Content;
                if (role == null) role = data.Message.Role;
                messageStringB.Append(msg);
                Response(msg,data.Done);
            });
            AppendChatMessage(new() { Role = role, Content = messageStringB.ToString() });
        }



        /// <summary>
        /// 获取上下文历史
        /// </summary>
        public virtual IEnumerable<OlllamaChatRoleMessage> GetChatMessages()
        {
            return ChatMessages;
        }
        public virtual IEnumerable<OlllamaChatRoleMessage> AppendChatMessage(OlllamaChatRoleMessage Msg)
        {
            ChatMessages.Add(Msg);
            return ChatMessages;

        }

    }
}
