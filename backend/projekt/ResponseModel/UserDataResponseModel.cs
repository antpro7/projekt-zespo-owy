using projekt.Models;

namespace projekt.ResponseModel
{
    public class UserDataResponseModel
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; }
        public string? Position { get; set; }
        public string? ContractType { get; set; }
        public int ManagerId { get; set; }
        public int MonthlyHours { get; set; } = 160;
        public int LeaveBalance { get; set; } = 26;
        public bool ChangePasswordRequired { get; set; }
        public UserDataResponseModel(User user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Role = user.Role;
            this.Position = user.Position ?? string.Empty;
            this.ContractType = user.ContractType ?? string.Empty;
            this.MonthlyHours = user.MonthlyHours;
            this.LeaveBalance = user.LeaveBalance;
            this.ManagerId = user.ManagerId ?? 0;
            this.ChangePasswordRequired = user.ChangePasswordRequired;

        }
    }
}
