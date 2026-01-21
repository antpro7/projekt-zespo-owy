using System.ComponentModel.DataAnnotations;

namespace projekt.Models
{
    public class UserAuth
    {
        [Key]
        public long Id { get; set; }
        public int UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
        public bool RefreshTokenIsRevoked { get; set; }
    }
}
