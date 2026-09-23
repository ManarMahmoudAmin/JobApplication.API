using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<IActionResult> AddJob(CreateJobDto createJobDto)
        {
            var recruiterIdClaim =
                    User.FindFirstValue("RecruiterId");

            if (recruiterIdClaim == null)
                return Forbid();

            var recruiterId = int.Parse(recruiterIdClaim);

            var job = await _jobService.AddJobAsync(createJobDto,recruiterId);
            return Ok(job);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _jobService.GetJobAsync(id);

            if(job == null)
                return NotFound(
                new { Message = $"Job with ID {id} not found." });
            return Ok(job);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}/close")]
        public async Task<IActionResult> CloseJob(int id)
        {
            var recruiterIdClaim =
                User.FindFirstValue("RecruiterId");

            if (recruiterIdClaim == null)
                return Forbid();

            var recruiterId = int.Parse(recruiterIdClaim);

            try
            {
                await _jobService.CloseAsync(id, recruiterId);

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
