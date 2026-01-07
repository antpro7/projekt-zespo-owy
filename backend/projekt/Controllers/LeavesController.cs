using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeavesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Pobieranie wszystkich wniosków (dla admina i managera)
        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            var leaves = await _context.LeaveRequests
                .Include(l => l.User) // Dołącza dane użytkownika (imię, nazwisko)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return Ok(leaves);
        }

        // 2. Pobieranie wniosków konkretnego użytkownika (Historia pracownika)
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLeaves(int userId)
        {
            var leaves = await _context.LeaveRequests
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return Ok(leaves);
        }

        // 3. Składanie nowego wniosku
        [HttpPost]
        public async Task<IActionResult> CreateLeave([FromBody] LeaveRequest request)
        {
            if (request == null) return BadRequest();

            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            request.Status = "Pending";

            // Ważne: Ustawiamy User na null przy dodawaniu, 
            // aby EF nie próbował stworzyć nowego użytkownika w bazie
            request.User = null;

            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Wniosek został złożony pomyślnie" });
        }

        // 4. AKTUALIZACJA STATUSU (Tego brakowało managerowi)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
            {
                return NotFound(new { message = "Nie znaleziono wniosku" });
            }

            // Walidacja statusu (opcjonalnie)
            if (status != "Approved" && status != "Rejected" && status != "Pending")
            {
                return BadRequest(new { message = "Nieprawidłowy status" });
            }

            leave.Status = status;
            leave.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Status został pomyślnie zaktualizowany" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Błąd bazy danych", error = ex.Message });
            }
        }
    }
}