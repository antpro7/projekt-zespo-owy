using projekt.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt.Models // Zmieñ na nazwê swojego projektu
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Role { get; set; } = "employee"; // admin, manager, employee
        public string? Position { get; set; }
        public string? ContractType { get; set; }

        public int MonthlyHours { get; set; } = 160;
        public int LeaveBalance { get; set; } = 26;

        // Relacja do Managera (Self-referencing)
        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual User? Manager { get; set; }

        public bool ChangePasswordRequired { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Lista wniosków danego u¿ytkownika
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}