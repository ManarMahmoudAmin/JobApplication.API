using JobApplication.Application.Interfaces;
using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.CandidateApplications.Commands.CancelApplication
{
    internal class CancelApplicationHandler : IRequestHandler<CancelApplicationCommand>
    {
        private readonly IApplicationRepository _applicationRepository;

        public CancelApplicationHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public async Task Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByIdAsync(request.ApplicationId);

            if (application == null)
                throw new InvalidOperationException("Application not found.");

            if (application.CandidateId != request.CandidateId)
                throw new UnauthorizedAccessException("You are not the owner of this application.");

            if (application.ApplicationStatus != ApplicationStatus.Applied &&
                application.ApplicationStatus != ApplicationStatus.UnderReview)
                throw new InvalidOperationException(
                    "Application cannot be cancelled.");

            application.ApplicationStatus = ApplicationStatus.Cancelled;
            application.CancelledAt = DateTime.UtcNow;
            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();
        }
    }
}
