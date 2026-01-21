using projekt.Helpers;
using projekt.Models;

namespace projekt.ResponseModel
{
    public class UserResponseModel
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool ChangePasswordRequired { get; set; }
        public AuthModel Tokens { get; set; } = new AuthModel();

        public UserResponseModel(User user,AuthModel auth)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Role = user.Role;
            this.Tokens = auth;
            this.ChangePasswordRequired = user.ChangePasswordRequired;
        }
    }
}
