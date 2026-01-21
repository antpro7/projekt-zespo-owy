namespace projekt.RequestModel
{
    public class RefreshTokenRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
