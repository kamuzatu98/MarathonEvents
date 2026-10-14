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
    public class ResultsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Organiser Only: Capture or update race finish position & time
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CaptureResult([FromBody] CaptureResultDto dto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role)) return Unauthorized(new { message = "Log in required." });
            if (role != "Organiser") return StatusCode(403, new { message = "Only Organisers can capture race results." });
            // Find existing result by enrollment id
            var existing = await _context.Results.FirstOrDefaultAsync(r => r.EnrollmentId == dto.EnrollmentId);

            if (existing == null)
            {
                var r = new MarathonEvents.Models.Results
                {
                    EnrollmentId = dto.EnrollmentId,
                    finishTime = dto.FinishTime,
                    finishPosition = dto.FinishPosition,
                    recordedTime = DateTime.UtcNow.ToString("o"),
                    PublishedAt = dto.IsPublished ? DateTime.UtcNow.ToString("o") : string.Empty
                };
                _context.Results.Add(r);
            }
            else
            {
                existing.finishTime = dto.FinishTime;
                existing.finishPosition = dto.FinishPosition;
                existing.recordedTime = DateTime.UtcNow.ToString("o");
                if (dto.IsPublished && string.IsNullOrEmpty(existing.PublishedAt)) existing.PublishedAt = DateTime.UtcNow.ToString("o");
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Result recorded successfully." });
        }

        /// <summary>
        /// Participant: View personal race history and results
        /// </summary>
        [HttpGet("my-results")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized(new { message = "Log in required." });

            string participantId = userId.Value.ToString();

            var myResults = await (from e in _context.Enrollments
                                   where e.Participant_Id == participantId
                                   join ev in _context.Events on e.EventId equals ev.EventId
                                   join r in _context.Results on e.EnrollmentId equals r.EnrollmentId
                                   where !string.IsNullOrEmpty(r.PublishedAt)
                                   select new
                                   {
                                       EventName = ev.Name,
                                       EventDate = ev.createAt,
                                       // Category not stored on Enrollment model currently
                                       FinishTime = r.finishTime,
                                       FinishPosition = r.finishPosition
                                   }).ToListAsync();

            return Ok(myResults);
        }
    }
}