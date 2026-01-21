namespace projekt.Helpers
{
    public class AuthModel
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public int RefreshTokenExpiresIn { get; set; }
    }
}
