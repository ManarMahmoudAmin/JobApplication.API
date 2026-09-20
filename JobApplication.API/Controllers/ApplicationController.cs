using JobApplication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost("apply/{jobId}")]
        public async Task<IActionResult> Apply(int jobId)
        {
            var candidateIdClaim = User.FindFirstValue("CandidateId");
            var candidateId = int.Parse(candidateIdClaim);
            var application =await _applicationService.Apply(candidateId, jobId);

            return Created("", application);
        }
    }
}
