namespace projekt
{
    public class AuthConfig
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string RefreshAudience { get; set; } = string.Empty;
        public string Key { get; set; }
        public int AccessTokenDuration { get; set; }
        public int RefreshTokenDuration { get; set; }
    }
}
