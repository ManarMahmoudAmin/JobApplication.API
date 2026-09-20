using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<CandidateApplicationDto> Apply(int candidateId, int jobId)
        {
            var job = await _applicationRepository.GetJobByIdAsync(jobId);

            //Check if the job is available
            if (job == null) 
                throw new InvalidOperationException("Job not found.");

            if (!job.IsActive)
                throw new InvalidOperationException(
                    "You cannot apply for this job anymore.");

            // Check if the application already exists
            await  ExistsAsync(candidateId, jobId);

            // Create a new application
            var application = new CandidateApplication
            {
                CandidateId = candidateId,
                JobId = jobId,
                AppliedAt = DateTime.UtcNow,
                ApplicationStatus = ApplicationStatus.Applied,
                StatusUpdatedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return new CandidateApplicationDto
            {
                CandidateId = application.CandidateId,
                JobId = application.JobId,
                ApplicationStatus = application.ApplicationStatus,
                AppliedAt = application.AppliedAt,
                StatusUpdatedAt = application.StatusUpdatedAt
            };
        }

        public async Task UpdateStatus(int candidateId, int jobId, ApplicationStatus newStatus, int recruiterId)
        {
            // Get the application
            var application = await _applicationRepository.GetApplicationAsync(candidateId, jobId);

            // Check if the application exists
            if (application == null)
                throw new InvalidOperationException("Application not found.");

            // Check if the recruiter owns the job
            if (application.Job.RecruiterId != recruiterId)
                throw new UnauthorizedAccessException("You are not the owner of this job.");

            // Update application status
            application.ApplicationStatus = newStatus;
            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();
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
