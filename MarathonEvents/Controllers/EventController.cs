using System;
using System.Linq;
using System.Threading.Tasks;
using MarathonEvents.Data;
using MarathonEvents.Models;
using MarathonEvents.API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarathonEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Helper method to verify session authentication and role
        private (bool IsAuthenticated, int UserId, string Role) GetSessionUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (userId == null || string.IsNullOrEmpty(role))
            {
                return (false, 0, string.Empty);
            }

            return (true, userId.Value, role);
        }

        /// <summary>
        /// Public / Shared: View all events with their associated categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Events
                .Select(e => new
                {
                    e.EventId,
                    e.Name,
                    e.Description,
                    CreatedAt = e.createAt,
                    UpdatedAt = e.updateAt,
                    e.Location,
                    e.Distance,
                    EventType = e.Event_Type,
                    BannerImage = e.BannerImage,
                    OrganizerId = e.OrganizerId
                })
                .ToListAsync();

            return Ok(events);
        }

        /// <summary>
        /// Public / Shared: View single event by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var raceEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (raceEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            return Ok(new
            {
                raceEvent.EventId,
                raceEvent.Name,
                raceEvent.Description,
                CreatedAt = raceEvent.createAt,
                UpdatedAt = raceEvent.updateAt,
                raceEvent.Location,
                raceEvent.Distance,
                EventType = raceEvent.Event_Type,
                BannerImage = raceEvent.BannerImage,
                OrganizerId = raceEvent.OrganizerId
            });
        }

        /// <summary>
        /// Organiser Only: Create a new event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var user = GetSessionUser();

            if (!user.IsAuthenticated)
            {
                return Unauthorized(new { message = "Authentication required. Please log in." });
            }

            if (user.Role != "Organiser")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Only Organisers can create events." });
            }

            var raceEvent = new Event
            {
                OrganizerId = user.UserId.ToString(),
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                Distance = dto.Distance,
                Event_Type = dto.EventType,
                BannerImage = dto.BannerImageUrl,
                createAt = DateTime.UtcNow.ToString("o")
            };

            _context.Events.Add(raceEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventById), new { id = raceEvent.EventId }, raceEvent);
        }

        /// <summary>
        /// Organiser Only: Update an existing event
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] CreateEventDto dto)
        {
            var user = GetSessionUser();

            if (!user.IsAuthenticated)
            {
                return Unauthorized(new { message = "Authentication required. Please log in." });
            }

            if (user.Role != "Organiser")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Only Organisers can edit events." });
            }

            var raceEvent = await _context.Events.FindAsync(id);

            if (raceEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Verify that the logged-in organiser owns this event
            if (raceEvent.OrganizerId != user.UserId.ToString())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. You can only update your own events." });
            }

            raceEvent.Name = dto.Name;
            raceEvent.Description = dto.Description;
            raceEvent.Location = dto.Location;
            raceEvent.Distance = dto.Distance;
            raceEvent.Event_Type = dto.EventType;
            raceEvent.BannerImage = dto.BannerImageUrl;
            raceEvent.updateAt = DateTime.UtcNow.ToString("o");

            await _context.SaveChangesAsync();

            return Ok(new { message = "Event updated successfully.", raceEvent });
        }

        /// <summary>
        /// Organiser Only: Delete an event
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var user = GetSessionUser();

            if (!user.IsAuthenticated)
            {
                return Unauthorized(new { message = "Authentication required. Please log in." });
            }

            if (user.Role != "Organiser")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Only Organisers can delete events." });
            }

            var raceEvent = await _context.Events.FindAsync(id);

            if (raceEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            if (raceEvent.OrganizerId != user.UserId.ToString())
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. You can only delete your own events." });
            }

            _context.Events.Remove(raceEvent);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event deleted successfully." });
        }
    }
}