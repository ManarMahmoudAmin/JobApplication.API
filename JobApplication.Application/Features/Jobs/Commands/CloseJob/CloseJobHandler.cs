using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commands.CloseJob
{
    internal class CloseJobHandler : IRequestHandler<CloseJobCommand>
    {
        private readonly IGenericRepository<Job> _jobRepository;
        public CloseJobHandler(IGenericRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task Handle(CloseJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);

            if (job == null)
                throw new InvalidOperationException(
                    "Job not found.");

            if (job.RecruiterId != request.RecruiterId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this job.");

            // Check if the job is already closed
            if (!job.IsActive)
                throw new InvalidOperationException(
                    "Job is already closed.");

            job.IsActive = false;
            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = request.RecruiterId;

            _jobRepository.Update(job);
            await _jobRepository.SaveChangesAsync();
        }
    }
}
