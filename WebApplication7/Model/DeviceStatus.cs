namespace Controls.Net7.Api.Model
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public class DeviceStatus
    {
        public string? Name { get; set; }
        public string? Ip { get; set; }
        public DateTime? Time { get; set; }
        public string? Status { get; set; }
    }
}
