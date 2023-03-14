using System;
using System.Collections.Generic;
using System.Text;

namespace Controls.Models
{
    public class PcStatus
    {
        public PcCmd PcStatu { get; set; }
        public PcCmd PcCmd { get; set; } 
        public string TimeAdd { get; set; } 

        public string PcIp { get; set; }
        public string PcName { get; set; } 
        public string PcLoginName { get; set; } 
        public string Time { get; set; }
        public string Other { get; set; } 
    }
    public enum PcCmd
    {
        Check,
        ServerUpdate,
        TurnOn,
        TurnOff,
        AddTime
    }
}
