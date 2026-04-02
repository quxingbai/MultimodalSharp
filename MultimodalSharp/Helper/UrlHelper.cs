using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimodalSharp.Helper
{
    public class UrlHelper
    {
        private String UrlSource { get; set; }
        public UrlHelper(string Url)
        {
            Url = Url.Replace('\\', '/');
            if (Url[Url.Length - 1] == '/') Url = Url.Substring(0, Url.Length - 2);
            this.UrlSource = Url;
        }
        public string AppendPath(params string[] Paths)
        {
            StringBuilder sb = new StringBuilder(UrlSource);
            foreach (var path in Paths)
            {
                var npath = path.Replace('\\', '/');
                if (npath[0] != '/') sb.Append('/');
                sb.Append(path);
            }
            return sb.ToString();
        }
    }
}
