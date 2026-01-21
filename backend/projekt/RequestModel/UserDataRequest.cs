namespace projekt.RequestModel
{
    public class UserDataRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Role { get; set; } = "employee";
        public string? Position { get; set; }
        public string? ContractType { get; set; }

        public int MonthlyHours { get; set; } = 160;
        public int LeaveBalance { get; set; } = 26;
        public int? ManagerId { get; set; }
    }
}
