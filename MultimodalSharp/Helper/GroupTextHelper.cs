using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultimodalSharp.Helper
{
    /// <summary>
    /// 结构化字符串用的，提示词啥的
    /// </summary>
    public class GroupTextHelper
    {
        private record class GroupItem(String GroupName, IEnumerable<string> GroupTexts);
        private List<GroupItem> GroupItems = new();
        private string? _StringTemp = null;
        public IEnumerable<String>? GetGrpuText(String GroupName)
        {
            var query = GroupItems.Where(w => w.GroupName == GroupName);
            if (query.Any())
            {
                return query.First().GroupTexts;
            }
            else
            {
                return null;
            }
        }
        public void SetGroupText(String GroupName, IEnumerable<String> GroupTexts)
        {
            var query = GroupItems.Where(w => w.GroupName == GroupName);
            if (query.Any()) GroupItems.Remove(query.First());
            GroupItems.Add(new(GroupName, GroupTexts));
            _StringTemp = null;
        }
        public void SetGroupText(string GroupName, params string[] GroupTexts)
        {
            SetGroupText(GroupName, GroupTexts.ToList());
        }
        public override string ToString()
        {
            return _StringTemp ?? (_StringTemp = CreateText());
        }
        private string CreateText()
        {
            //StringBuilder sb = new();
            //foreach (var i in GroupItems)
            //{
            //    sb.Append("$");
            //    sb.Append(i.GroupName);
            //    sb.Append(":");
            //    sb.AppendLine();
            //    foreach (var t in i.GroupTexts)
            //    {
            //        sb.Append(" ");
            //        sb.AppendLine(t);
            //    }
            //}
            //return sb.ToString();
            return JsonSerializer.Serialize(GroupItems);
        }
    }
}
