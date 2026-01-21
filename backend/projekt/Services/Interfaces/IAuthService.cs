using projekt.Helpers;
using projekt.Models;

namespace projekt.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> VerifyPassword(int userId, string password);
        Task<User> Authenticate(string email, string password);
        Task<bool> ValidateToken(int userId, string token);
        Task<bool> ValidateRefreshToken(int userId, string refreshToken);
        Task<AuthModel> GenerateTokensForUser(User user);
        Task<AuthModel> RefreshTokens(int userId, string refreshToken);
        Task<bool> RevokeTokens(int userId);
        Task<bool> ChangePassword(int userId,string oldPassword, string newPassword);
    }
}