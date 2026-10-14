using System;
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
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Participant Only: Enroll into an event
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (userId == null) return Unauthorized(new { message = "Log in required." });
            if (role != "Participant") return StatusCode(403, new { message = "Only Participants can enroll in events." });

            // Check if already enrolled (Enrollment.Participant_Id is stored as string)
            string participantIdStr = userId.Value.ToString();
            bool exists = await _context.Enrollments
                .AnyAsync(e => e.Participant_Id == participantIdStr && e.EventId == dto.EventId);

            if (exists) return BadRequest(new { message = "You are already enrolled in this event." });

            var enrollment = new Enrollment
            {
                Participant_Id = participantIdStr,
                EventId = dto.EventId,
                // Enrollment model doesn't currently have a CategoryId property; skip storing category selection
                EnrollmentStatus = "Confirmed",
                EnrolledAt = DateTime.UtcNow.ToString("o")
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Enrolled successfully!", enrollment });
        }

        /// <summary>
        /// Organiser Only: View all participants enrolled in a specific event
        /// </summary>
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventEnrollments(int eventId)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role)) return Unauthorized(new { message = "Log in required." });
            if (role != "Organiser") return StatusCode(403, new { message = "Only Organisers can view full event enrollments." });

            var enrollments = await _context.Enrollments
                .Where(e => e.EventId == eventId)
                .Select(e => new
                {
                    e.EnrollmentId,
                    ParticipantId = e.Participant_Id,
                    // CategoryId not present on model; return placeholder if needed
                    e.EnrollmentStatus,
                    e.EnrolledAt
                })
                .ToListAsync();

            return Ok(enrollments);
        }
    }
}