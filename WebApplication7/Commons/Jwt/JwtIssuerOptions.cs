namespace Controls.Net7.Api.Commons.Jwt
{
    /// <summary>
    /// JWT配置项
    /// </summary>
    public class JwtIssuerOptions
    {
        /// <summary>
        /// JWT发行人
        /// </summary>
        public string? Issuer { get; set; }
        /// <summary>
        /// JWT订阅人
        /// </summary>
        public string? Audience { get; set; }

        public string? ValidFor { get; set; }
        public string? ValidAudience { get; set; }
        public string? SecurityKey { get; set; }
    }
}
