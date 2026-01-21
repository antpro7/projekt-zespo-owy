using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;
using projekt.RequestModel;
using projekt.ResponseModel;

namespace projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Pobieranie wszystkich pracowników (Lista pracowników)
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                // Pobieramy wszystkich użytkowników
                var users = await _context.Users.ToListAsync();
                var responseModel = new List<UserDataResponseModel>();
                foreach (var user in users)
                {
                    responseModel.Add(new UserDataResponseModel(user));
                }
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd bazy danych: {ex.Message}");
            }
        }

        // 2. Pobieranie jednego pracownika po ID (Potrzebne do profilu!)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                // .Include(u => u.Manager) pozwoli wyświetlić imię szefa na profilu
                var user = await _context.Users
                    .Include(u => u.Manager)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound(new { message = "Nie znaleziono użytkownika" });
                }

                return Ok(new UserDataResponseModel(user));
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Błąd serwera: {ex.Message}");
            }
        }

        // 3. Aktualizacja danych użytkownika (Opcjonalne - do edycji profilu)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDataRequest updatedUser)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Aktualizujemy tylko wybrane pola
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Position = updatedUser.Position;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new UserDataResponseModel(user));

        }
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] UserDataRequest newUser)
        {
            try
            {
                var newUserEntity = new User
                {
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Role = newUser.Role,
                    Position = newUser.Position,
                    ContractType = newUser.ContractType,
                    MonthlyHours = newUser.MonthlyHours,
                    LeaveBalance = newUser.LeaveBalance,
                    ManagerId = newUser.ManagerId
                };

                newUserEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword("password");
                newUserEntity.ChangePasswordRequired = true;
                newUserEntity.CreatedAt = DateTime.Now;
                newUserEntity.UpdatedAt = DateTime.Now;
                _context.Users.Add(newUserEntity);
                await _context.SaveChangesAsync();
                return Ok(new UserDataResponseModel(newUserEntity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd serwera: {ex.Message}");
            }
        }
    }
}