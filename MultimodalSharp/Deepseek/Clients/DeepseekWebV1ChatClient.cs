using MultimodalSharp.Abstractions.Entities;
using MultimodalSharp.Abstractions.Interfaces;
using MultimodalSharp.Deepseek.Models;
using MultimodalSharp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MultimodalSharp.Deepseek.Models.DeepSeekModels;

namespace MultimodalSharp.Deepseek.Clients
{
    public class DeepseekWebV1ChatClient : ChatMessageSendModelBase<DeepSeekChatRequestModel, DeepSeekChatResponseModel, DeepSeekChatMessage>, ITTLChatCompletion<DeepSeekChatMessage>
    {
        private String ApiKey { get; set; }
        public DeepseekWebV1ChatClient(HttpClient Http, String ApiKey, string BaseUrl = "https://api.deepseek.com/chat/completions") : base(null, Http, BaseUrl)
        {
            this.ApiKey = ApiKey;
            Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }
        /// <summary>
        /// 全文本请求
        /// </summary>
        public async Task<string> RequestMessageAsync(string Message)
        {
            var data = await RequestMessageAsync(new DeepSeekChatRequestModel
            {
                Model = "deepseek-chat",
                Messages = AppendChatMessage(new DeepSeekChatMessage
                {
                    Role = "user",
                    Content = Message
                })
            });
            var message = data.Choices[0].Message;
            AppendChatMessage(message);
            var text = message.Content;
            return text;
        }
        /// <summary>
        /// 发送流返回请求
        /// </summary>
        public async Task RequestMessageAsync(string Message, StreamMessageData Response, CancellationToken? CancelToekn = null)
        {
            StringBuilder sb = new();
            string role = null;
            await RequestMessageAsync(new DeepSeekChatRequestModel
            {
                Model = "deepseek-chat",
                Messages = AppendChatMessage(new DeepSeekChatMessage
                {
                    Role = "user",
                    Content = Message
                }),
                Stream = true
            }, (data) =>
            {
                var msg = data.Choices[0].Delta?.Content ?? "";
                if (role == null) role = data.Choices[0].Delta.Role;
                sb.Append(msg);
                Response(msg, data.Choices[0].FinishReason == "stop");
            }, CancelToekn);
            AppendChatMessage(new DeepSeekChatMessage()
            {
                Content = sb.ToString(),
                Role = role
            });
        }
        /// <summary>
        /// 发送流返回请求
        /// </summary>
        public async Task RequestMessageAsync(DeepSeekChatRequestModel RequestModel, Action<DeepSeekChatResponseModel> Response, CancellationToken? CancelToken = null)
        {
            await base.SendReadStream<DeepSeekChatResponseModel>(Http, CreateRequestMessage(RequestModel), (data) =>
              {
                  if (data != null) Response(data);
              }, CancelToken, text =>
              {
                  if (text == "data: [DONE]")
                  {
                      return null;
                  }
                  else
                  {
                      string json = text[5..];
                      return JsonSerializer.Deserialize<DeepSeekChatResponseModel>(json);
                  }
              });
        }
        public async Task<DeepSeekChatResponseModel> RequestMessageAsync(DeepSeekChatRequestModel RequestModel) => await base.PostRequestMessageAsync(RequestModel);



        public IEnumerable<DeepSeekChatMessage> AppendChatMessage(params DeepSeekChatMessage[] Msgs) => base.AppendChatContextMessage(Msgs);

        public void InitChatMessages(params DeepSeekChatMessage[] Msg) => base.InitChatContextMessages(Msg);

        public IEnumerable<DeepSeekChatMessage> GetChatMessages() => base.GetChatContextMessages();

        /// <summary>
        /// 创建一个默认请求
        /// </summary>
        protected HttpRequestMessage CreateRequestMessage(DeepSeekChatRequestModel RequestModel)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = HttpHelper.CreateJsonContent(RequestModel, true),
            };
            req.Headers.Add("Authorization", $"Bearer {ApiKey}");
            return req;
        }
    }
}
