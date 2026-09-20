using JobApplication.Application.Interfaces;
using JobApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost("apply/{jobId}")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Apply(int jobId)
        {
            var candidateIdClaim = User.FindFirstValue("CandidateId");
            var candidateId = int.Parse(candidateIdClaim);
            var application =await _applicationService.Apply(candidateId, jobId);

            return Created("", application);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{candidateId}/{jobId}/status")]
        public async Task<IActionResult> UpdateStatus(int candidateId, int jobId, ApplicationStatus newStatus)
        {
            var recruiterIdClaim = User.FindFirstValue("RecruiterId");

            if (recruiterIdClaim == null)
                return Forbid();

            var recruiterId = int.Parse(recruiterIdClaim);

            try
            {
                await _applicationService.UpdateStatus(candidateId, jobId, newStatus, recruiterId);

                return Ok();
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Cancel(int id)
        {
            var candidateIdClaim =
                User.FindFirstValue("CandidateId");

            if (candidateIdClaim == null)
                return Forbid();

            var candidateId = int.Parse(candidateIdClaim);

            try
            {
                await _applicationService.Cancel(id, candidateId);

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
