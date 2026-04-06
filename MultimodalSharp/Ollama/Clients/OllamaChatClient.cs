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
        private Ollama_Chat_InitDataModel _InitData = null;
        public String MainSystemMessage { get; protected set; }//这条消息将会被扔到每次的request中作为system的提示
        public OllamaChatClient(Ollama_Chat_InitDataModel InitData) : base(InitData.ModelName, InitData.HttpClient, $"http://{InitData.ServerIP.Address}:{InitData.ServerIP.Port}/api/chat")
        {
            _InitData = InitData;
            //this.SystemMessage = InitData.SystemMessage;
        }

        /// <summary>
        /// 压缩上下文数据，然后放到MainSystemMessage中，每次发消息都携带此System提示消息
        /// </summary>
        /// <param name="CompressTextMessage">压缩后的消息，可以为null 将会自动推理压缩内容</param>
        public async Task SetCompressContext(String CompressTextMessage = null)
        {
            if (CompressTextMessage == null) CompressTextMessage = await GetCompressedContext();
            MainSystemMessage = CompressTextMessage;
        }
        public async Task<string> GetCompressedContext()
        {
            var data = await RequestMessageAsync(new OllamaChatRequestModel()
            {
                Model = ModelName,
                Messages = AppendChatContextMessageCanSave(OlllamaChatRoleMessage.CreateUserMessage("将上面的对话历史压缩成一条上下文快照。不能损失对话内容 尽量无损压缩。"), OlllamaChatRoleMessage.CreateSystemMessage("你是一个专业的对话摘要助手，擅长从对话中提取关键信息并精炼表达。"))
            });
            return data.Message.Content;
        }


        /// <summary>
        /// 发文本消息 一次性接收所有回复文本，自动维护上下文历史
        /// </summary>
        public async Task<string> RequestMessageAsync(string Message)
        {
            var data = await RequestMessageAsync(new OllamaChatRequestModel()
            {
                Model = ModelName,
                Messages = AppendChatContextMessageCanSave(OlllamaChatRoleMessage.CreateUserMessage(Message), MainSystemMessage==null?null:OlllamaChatRoleMessage.CreateSystemMessage(MainSystemMessage),true)
            });
            var message = new OlllamaChatRoleMessage()
            {
                Role = data.Message.Role,
                Content = data.Message.Content
            };
            AppendChatContextMessages(message);
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
                Messages = AppendChatContextMessageCanSave(OlllamaChatRoleMessage.CreateUserMessage(Message), MainSystemMessage == null ? null : OlllamaChatRoleMessage.CreateSystemMessage(MainSystemMessage), true)
            }, (data) =>
            {
                var msg = data.Message.Content;
                if (role == null) role = data.Message.Role;
                messageStringB.Append(msg);
                Response(msg, data.Done);
            }, CancelToekn);

            base.AppendChatContextMessages(new OlllamaChatRoleMessage() { Role = role, Content = messageStringB.ToString() });
        }
        /// <summary>
        /// 发模型消息 一次性接收所有回复文本。需要手动维护上下文
        /// </summary>
        public async Task<OllamaChatResponseModel> RequestMessageAsync(OllamaChatRequestModel RequestModel) => await PostRequestMessageAsync(RequestModel);

        /// <summary>
        /// 发送模型消息 流式接收回复文本。需要手动维护上下文
        /// </summary>
        public async Task RequestMessageAsync(OllamaChatRequestModel RequestModel, Action<OllamaChatResponseModel> Response, CancellationToken? CancelToekn = null) => await PostRequestMessageStreamAsync(RequestModel, Response, CancelToekn);

    }
}
