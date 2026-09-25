using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IGenericRepository<CandidateApplication> _jobCandidateApplicationRepository;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IGenericRepository<CandidateApplication> jobCandidateApplicationRepository, ILogger<EmailNotificationService> logger)
        {
            _jobCandidateApplicationRepository = jobCandidateApplicationRepository;
            _logger = logger;
        }

        public async Task NotifyRecruiter(int applicationId)
        {
            var application = await _jobCandidateApplicationRepository.GetByIdAsync(applicationId);

            if (application is null)
            {
                _logger.LogWarning("application {applicationId}is not found ", applicationId);
                return;
            }
            _logger.LogInformation("Send Email :  cadidate {CandidateId} has applied to {JobId} and applicationId is {applicationId}",
                application.CandidateId, application.JobId, applicationId);
        }
    }
}
