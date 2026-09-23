using JobApplication.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.CandidateApplications.Commands.Apply
{
    public class ApplyCommand : IRequest<CandidateApplicationDto>
    {
        public int CandidateId { get; set; }

        public int JobId { get; set; }

    }
}
