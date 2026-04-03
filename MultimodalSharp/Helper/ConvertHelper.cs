using Microsoft.Extensions.AI;
using MultimodalSharp.Ollama.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ollamaRequest = MultimodalSharp.Ollama.Models.Entities.OllamaRequests;

namespace MultimodalSharp.Helper
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 将微软的 ChatMessage 转换为你的 OllamaMessage
        /// </summary>
        public static ollamaRequest.OlllamaChatRoleMessage ToOllamaMessage(ChatMessage chatMessage)
        {
            var ollamaMessage = new ollamaRequest.OlllamaChatRoleMessage
            {
                Role = chatMessage.Role.Value,
                Content = string.Empty,
                Images = new List<string>()
            };

            foreach (var content in chatMessage.Contents)
            {
                switch (content)
                {
                    case TextContent textContent:
                        ollamaMessage.Content += textContent.Text;
                        break;

                    case DataContent dataContent:
                        // DataContent 用于处理图片、音频等二进制数据
                        // 检查媒体类型是否为图片
                        if (dataContent.MediaType?.StartsWith("image/") == true)
                        {
                            string base64Image = ExtractBase64FromDataContent(dataContent);
                            ollamaMessage.Images.Add(base64Image);
                        }
                        break;
                }
            }

            if (ollamaMessage.Content == string.Empty && !string.IsNullOrEmpty(chatMessage.Text))
            {
                ollamaMessage.Content = chatMessage.Text;
            }

            return ollamaMessage;
        }
        /// <summary>
        /// 根据datacontent返回base64
        /// </summary>
        private static string ExtractBase64FromDataContent(DataContent dataContent)
        {
            var uri = new Uri(dataContent.Uri ?? "");
            // 情况1：已经有 Data URI（比如从上层传入的已经是 base64 字符串）
            if (dataContent.Uri != null && uri.Scheme == "data")
            {
                var dataUri = dataContent.Uri.ToString();
                var commaIndex = dataUri.IndexOf(',');
                if (commaIndex > 0 && commaIndex + 1 < dataUri.Length)
                {
                    // 直接提取 "xxxxx" 部分，不需要转换
                    return dataUri.Substring(commaIndex + 1);
                }
            }

            // 情况2：原始二进制数据（比如从 File.ReadAllBytes 传入）
            if (dataContent.Data.Length != 0 && dataContent.Data.Length > 0)
            {
                // 需要将 byte[] 转换为 base64
                return Convert.ToBase64String(dataContent.Data.Span);
            }

            throw new InvalidOperationException("无法提取图片数据");
        }

    }
}
