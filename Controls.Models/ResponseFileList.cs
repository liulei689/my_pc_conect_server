using System;
using System.Collections.Generic;

namespace Controls.Models
{
    public class ResponseFileList
    {
        public List<ResponseFile>? FileList { get; set; }
        public int Counts { get; set; }

    }
    public class ResponseFile
    {
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public DateTime? CreateTime { get; set; }

    }
}
