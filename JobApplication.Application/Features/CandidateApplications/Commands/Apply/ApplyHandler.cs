using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.CandidateApplications.Commands.Apply
{
    internal class ApplyHandler : IRequestHandler<ApplyCommand, CandidateApplicationDto>
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplyHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<CandidateApplicationDto> Handle(ApplyCommand request, CancellationToken cancellationToken)
        {
            var job = await _applicationRepository.GetJobByIdAsync(request.JobId);

            //Check if the job is available
            if (job == null)
                throw new InvalidOperationException("Job not found.");

            if (!job.IsActive)
                throw new InvalidOperationException(
                    "You cannot apply for this job anymore.");

            // Check if the application already exists
            await ExistsAsync(request.CandidateId, request.JobId);

            // Create a new application
            var application = new CandidateApplication
            {
                CandidateId = request.CandidateId,
                JobId = request.JobId,
                AppliedAt = DateTime.UtcNow,
                ApplicationStatus = ApplicationStatus.Applied,
                StatusUpdatedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return new CandidateApplicationDto
            {
                Id = application.Id,
                CandidateId = application.CandidateId,
                JobId = application.JobId,
                ApplicationStatus = application.ApplicationStatus,
                AppliedAt = application.AppliedAt,
                StatusUpdatedAt = application.StatusUpdatedAt
            };
        }

        private async Task ExistsAsync(int candidateId, int jobId)
        {
            var applied = await _applicationRepository.ExistsAsync(candidateId, jobId);
            if (applied)
            {
                throw new InvalidOperationException("Application already exists.");
            }
        }

    }
}
