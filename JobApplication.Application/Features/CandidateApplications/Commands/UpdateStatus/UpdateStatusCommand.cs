using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.CandidateApplications.Commands.UpdateStatus
{
    public class UpdateStatusCommand : IRequest
    {
        public int CandidateId { get; set; }
        public int JobId { get; set; }
        public ApplicationStatus NewStatus { get; set; }
        public int RecruiterId { get; set; }
    }
}
