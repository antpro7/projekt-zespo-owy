using Microsoft.EntityFrameworkCore;
using projekt.Models;

namespace projekt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tabele w bazie danych
        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji "Pracownik ma Managera" (self-referencing)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Manager)
                .WithMany()
                .HasForeignKey(u => u.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Konfiguracja autoincrement dla LeaveRequest.Id
            modelBuilder.Entity<LeaveRequest>()
                .Property(lr => lr.Id)
                .ValueGeneratedOnAdd();

            // Konfiguracja autoincrement dla User.Id (opcjonalnie, jeśli nie działa)
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // SEED DATA - Użytkownicy
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1, // Muszę podać ID dla seed data
                    Email = "admin@example.com",
                    PasswordHash = "password",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = "admin",
                    Position = "Administrator",
                    ContractType = "B2B",
                    MonthlyHours = 160,
                    LeaveBalance = 26,
                    ManagerId = null,
                    ChangePasswordRequired = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 2,
                    Email = "alice@example.com",
                    PasswordHash = "password",
                    FirstName = "Manager",
                    LastName = "Alice",
                    Role = "manager",
                    Position = "Team Lead 1",
                    ContractType = "UoP",
                    MonthlyHours = 160,
                    LeaveBalance = 26,
                    ManagerId = null,
                    ChangePasswordRequired = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 3,
                    Email = "emily@example.com",
                    PasswordHash = "password",
                    FirstName = "Manager",
                    LastName = "Emily",
                    Role = "manager",
                    Position = "Team Lead 2",
                    ContractType = "UoP",
                    MonthlyHours = 160,
                    LeaveBalance = 26,
                    ManagerId = null,
                    ChangePasswordRequired = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 4,
                    Email = "john@example.com",
                    PasswordHash = "password",
                    FirstName = "John",
                    LastName = "Doe",
                    Role = "employee",
                    Position = "Developer",
                    ContractType = "UoP",
                    MonthlyHours = 160,
                    LeaveBalance = 20,
                    ManagerId = 2, // Odnosi się do Alice (Manager)
                    ChangePasswordRequired = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 5,
                    Email = "jane@example.com",
                    PasswordHash = "password",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Role = "employee",
                    Position = "Designer",
                    ContractType = "UoP",
                    MonthlyHours = 160,
                    LeaveBalance = 20,
                    ManagerId = 3, // Odnosi się do Emily (Manager)
                    ChangePasswordRequired = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1)
                },
                new User
                {
                    Id = 6,
                    Email = "bob@example.com",
                    PasswordHash = "password",
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Role = "employee",
                    Position = "QA Engineer",
                    ContractType = "UoP",
                    MonthlyHours = 160,
                    LeaveBalance = 20,
                    ManagerId = 2, // Odnosi się do Alice (Manager)
                    ChangePasswordRequired = false,
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1)
                }
            );

            // SEED DATA - Wnioski urlopowe
            modelBuilder.Entity<LeaveRequest>().HasData(
                new LeaveRequest
                {
                    Id = 1, // Muszę podać ID dla seed data
                    UserId = 4, // John Doe
                    LeaveType = "Annual",
                    StartDate = new DateTime(2023, 12, 1),
                    EndDate = new DateTime(2023, 12, 5),
                    Status = "Pending",
                    Comment = "Urlop wypoczynkowy",
                    CreatedAt = new DateTime(2023, 11, 15),
                    UpdatedAt = new DateTime(2023, 11, 15)
                },
                new LeaveRequest
                {
                    Id = 2,
                    UserId = 5, // Jane Smith
                    LeaveType = "Sick",
                    StartDate = new DateTime(2023, 11, 20),
                    EndDate = new DateTime(2023, 11, 22),
                    Status = "Approved",
                    Comment = "Zwolnienie lekarskie",
                    CreatedAt = new DateTime(2023, 11, 19),
                    UpdatedAt = new DateTime(2023, 11, 21)
                },
                new LeaveRequest
                {
                    Id = 3,
                    UserId = 6, // Bob Johnson
                    LeaveType = "Annual",
                    StartDate = new DateTime(2023, 12, 10),
                    EndDate = new DateTime(2023, 12, 15),
                    Status = "Pending",
                    Comment = "Urlop świąteczny",
                    CreatedAt = new DateTime(2023, 11, 25),
                    UpdatedAt = new DateTime(2023, 11, 25)
                },
                new LeaveRequest
                {
                    Id = 4,
                    UserId = 5, // Jane Smith
                    LeaveType = "Annual",
                    StartDate = new DateTime(2023, 12, 20),
                    EndDate = new DateTime(2023, 12, 24),
                    Status = "Pending",
                    Comment = "Urlop przed świętami",
                    CreatedAt = new DateTime(2023, 12, 1),
                    UpdatedAt = new DateTime(2023, 12, 1)
                },
                new LeaveRequest
                {
                    Id = 5,
                    UserId = 4, // John Doe
                    LeaveType = "Sick",
                    StartDate = new DateTime(2023, 10, 1),
                    EndDate = new DateTime(2023, 10, 3),
                    Status = "Approved",
                    Comment = "Choroba",
                    CreatedAt = new DateTime(2023, 9, 30),
                    UpdatedAt = new DateTime(2023, 10, 2)
                },
                new LeaveRequest
                {
                    Id = 6,
                    UserId = 4, // John Doe
                    LeaveType = "Annual",
                    StartDate = new DateTime(2023, 10, 15),
                    EndDate = new DateTime(2023, 10, 18),
                    Status = "Approved",
                    Comment = "Długi weekend",
                    CreatedAt = new DateTime(2023, 10, 1),
                    UpdatedAt = new DateTime(2023, 10, 5)
                }
            );
        }
    }
}