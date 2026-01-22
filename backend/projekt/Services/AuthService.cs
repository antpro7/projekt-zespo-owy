using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using projekt.Data;
using projekt.Helpers;
using projekt.Models;
using projekt.Services.Interfaces;
using System.Text;

namespace projekt.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly AuthConfig _authConfig;
        public AuthService(AppDbContext context, IOptions<AuthConfig> options)
        {
            _context = context;
            _authConfig = options.Value;
        }
        public async Task<User> VerifyPassword(int userId, string password)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (foundUser == null)
            {
                return null;
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, foundUser.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            return foundUser!;
        }
        public async Task<User> Authenticate(string email, string password)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (foundUser == null)
            {
                return null;
            }
            return await VerifyPassword(foundUser.Id, password);
        }
        public async Task<bool> ValidateToken(string token)
        {
            var userAuth = await _context.UserAuths
                .AsNoTracking()
                .Where(ua => ua.TokenHash == token && !ua.IsRevoked && ua.ExpiresAt > DateTime.Now)
                .OrderByDescending(ua => ua.ExpiresAt)
                .FirstOrDefaultAsync();
            if (userAuth == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateRefreshToken(int userId, string refreshToken)
        {
            var userAuth = await _context.UserAuths
                .AsNoTracking()
                .Where(ua => ua.UserId == userId && !ua.RefreshTokenIsRevoked && ua.RefreshTokenExpiresAt > DateTime.Now)
                .OrderByDescending(ua => ua.RefreshTokenExpiresAt)
                .FirstOrDefaultAsync();
            if (userAuth == null)
            {
                return false;
            }
            return refreshToken == userAuth.RefreshTokenHash;
        }
        public async Task<bool> RevokeTokens(int userId)
        {
            var userAuths = await _context.UserAuths
                .Where(ua => ua.UserId == userId && (!ua.IsRevoked || !ua.RefreshTokenIsRevoked))
                .ToListAsync();
            if (userAuths.Count == 0)
            {
                return false;
            }
            foreach (var userAuth in userAuths)
            {
                _context.Remove(userAuth);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AuthModel> RefreshTokens(int userId, string refreshToken)
        {
            var isValid = await ValidateRefreshToken(userId, refreshToken);
            if (!isValid)
            {
                return null;
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            await RevokeTokens(userId);
            return await GenerateTokensForUser(user);
        }
        public async Task<AuthModel> GenerateTokensForUser(User user)
        {
            var accessToken = GenerateTokenForUser(user, _authConfig.AccessTokenDuration);
            var refreshToken = GenerateTokenForUser(user, _authConfig.RefreshTokenDuration, true);

            var userAuth = new UserAuth
            {
                UserId = user.Id,
                TokenHash = accessToken,
                ExpiresAt = DateTime.Now.AddSeconds(_authConfig.AccessTokenDuration),
                IsRevoked = false,
                RefreshTokenHash = refreshToken,
                RefreshTokenExpiresAt = DateTime.Now.AddSeconds(_authConfig.RefreshTokenDuration),
                RefreshTokenIsRevoked = false
            };
            _context.UserAuths.Add(userAuth);
            await _context.SaveChangesAsync();
            return new AuthModel
            {
                Token = accessToken,
                ExpiresIn = _authConfig.AccessTokenDuration,
                RefreshToken = refreshToken,
                RefreshTokenExpiresIn = _authConfig.RefreshTokenDuration
            };
        }
        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            if (await VerifyPassword(userId, oldPassword) != null)
            {
                return false;
            }
            string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordHash = newHashedPassword;
            user.ChangePasswordRequired = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        private string GenerateTokenForUser(User user, int duration, bool isrefresh = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim("id", user.Id.ToString()),
                new System.Security.Claims.Claim("email", user.Email),
                new System.Security.Claims.Claim("firstName", user.FirstName),
                new System.Security.Claims.Claim("lastName", user.LastName),
                new System.Security.Claims.Claim("role", user.Role)
            };
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(duration),
                signingCredentials: credentials,
                audience: isrefresh ? _authConfig.RefreshAudience : _authConfig.Audience,
                issuer: _authConfig.Issuer
            );
            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
