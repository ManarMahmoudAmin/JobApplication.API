using JobApplication.Application.Features.CandidateApplications.Commands.Apply;
using JobApplication.Application.Features.CandidateApplications.Commands.UpdateStatus;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IMediator _mediator;

        public ApplicationsController(IApplicationService applicationService, IMediator mediator)
        {
            _applicationService = applicationService;
            _mediator = mediator;
        }

        [HttpPost("apply/{jobId}")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> Apply(int jobId)
        {
            var candidateIdClaim = User.FindFirstValue("CandidateId");
            var candidateId = int.Parse(candidateIdClaim);
            //var application =await _applicationService.Apply(candidateId, jobId);
            var application = await _mediator.Send(new ApplyCommand()
            {
                CandidateId = candidateId,
                JobId = jobId
            });

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
                //await _applicationService.UpdateStatus(candidateId, jobId, newStatus, recruiterId);
                await _mediator.Send(new UpdateStatusCommand()
                {
                    CandidateId = candidateId,
                    JobId = jobId,
                    NewStatus = newStatus,
                    RecruiterId = recruiterId
                });

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
        public async Task<IActionResult> CancelApplication(int id)
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
