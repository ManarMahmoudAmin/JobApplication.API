using JobApplication.Application.DTOs;
using JobApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IApplicationService
    {
        Task<CandidateApplicationDto> Apply(int candidateId, int jobId);
        Task UpdateStatus(int candidateId, int jobId, ApplicationStatus newStatus, int recruiterId);
    }
}
