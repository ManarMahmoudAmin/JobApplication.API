using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.CandidateApplications.Commands.UpdateStatus
{
    internal class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand>
    {
        private readonly IApplicationRepository _applicationRepository;

        public UpdateStatusHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public async Task Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {

            // Get the application
            var application = await _applicationRepository.GetApplicationAsync(request.CandidateId, request.JobId);

            // Check if the application exists
            if (application == null)
                throw new InvalidOperationException("Application not found.");

            // Check if the recruiter owns the job
            if (application.Job.RecruiterId != request.RecruiterId)
                throw new UnauthorizedAccessException("You are not the owner of this job.");

            // Update application status
            application.ApplicationStatus = request.NewStatus;
            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();
        }
    }
}
