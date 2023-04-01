using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Controls.Models
{
    [Description("代码库")]
    public class Codess
    {
        [Description("唯一标识")]
        public string _id { get; set; }
        [Description("语言")]
        public string Language { get; set; } = "C#";
        [Description("用处")]
        public string Use { get; set; } = "";
        [Description("用处细节")]
        public string UseDetail { get; set; } = "";
        [Description("来源")]
        public string From { get; set; } = "";
        [Description("技术")]
        public string Technical { get; set; } = "Csharp工具代码";
        [Description("更新时间")]
        public string TimeUpate { get; set; } = "";
        [Description("创建时间")]
        public string CreateTime { get; set; } = "";
        [Description("阅读数")]
        public int ReadCount { get; set; } = 0;
        [Description("代码")]
        public string Code { get; set; } = "";
        [Description("访问时间")]
        public string ReadTime { get; set; } = "";

    }
}
