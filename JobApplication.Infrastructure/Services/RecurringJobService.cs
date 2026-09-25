using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Services
{
    public class RecurringJobService : IRecurringJobService
    {
        private readonly IGenericRepository<Job> _jobRepository;

        public RecurringJobService(IGenericRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task AutoCloseOldJobsAsync()
        {
            var cutoff = DateTime.UtcNow.AddDays(-30);

            var staleJobs = await _jobRepository.FindAsync(
                j => j.IsActive && j.CreatedAt < cutoff);

            foreach (var job in staleJobs)
            {
                job.IsActive = false;
                job.ClosedAt = DateTime.UtcNow;
                job.ClosedBy = null;

                _jobRepository.Update(job);
            }

            await _jobRepository.SaveChangesAsync();
        }
    }
}