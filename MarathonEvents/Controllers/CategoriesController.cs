using System.Linq;
using System.Threading.Tasks;
using MarathonEvents.Data;
using MarathonEvents.Models;
using MarathonEvents.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarathonEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context) => _context = context;

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetCategoriesByEvent(int eventId)
        {
            var categories = await _context.Categories
                .Where(c => c.EventId == eventId.ToString())
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role)) return Unauthorized(new { message = "Log in required." });
            if (role != "Organiser") return StatusCode(403, new { message = "Only Organisers can create categories." });

            var category = new Category
            {
                EventId = dto.EventId.ToString(),
                CategoryName = dto.Name,
                CategoryType = dto.CategoryType,
                minAge = dto.MinAge ?? 0,
                maxAge = dto.MaxAge ?? 0,
                Distance = dto.DistanceKm?.ToString() ?? string.Empty
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}