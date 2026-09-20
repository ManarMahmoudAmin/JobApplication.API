using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Authorize(Roles = "Recruiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter")]
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
    }
}
