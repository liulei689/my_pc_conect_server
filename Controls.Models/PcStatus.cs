using System;
using System.Collections.Generic;
using System.Text;

namespace Controls.Models
{
    public class PcStatus
    {
        public string PcStatu { get; set; } = "开机";
        public string PcCmd { get; set; } = "检查开机";

        public string PcIp { get; set; } = "127.0.0.1";
        public string PcName { get; set; } = "";
        public string PcLoginName { get; set; } = "";
        public DateTime Time { get; set; }
        public string Other { get; set; } = "";
    }
}
