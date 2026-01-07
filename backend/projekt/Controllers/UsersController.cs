using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

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
                return Ok(users);
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

                return Ok(user);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Błąd serwera: {ex.Message}");
            }
        }

        // 3. Aktualizacja danych użytkownika (Opcjonalne - do edycji profilu)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id) return BadRequest();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Aktualizujemy tylko wybrane pola
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Position = updatedUser.Position;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}