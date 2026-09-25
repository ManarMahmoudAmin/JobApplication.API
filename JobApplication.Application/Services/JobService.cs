using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IGenericRepository<Job> _jobRepository;
        private readonly IGenericRepository<Recruiter> _recruiterRepository;

        public JobService(IGenericRepository<Job> repo, IGenericRepository<Recruiter> recruiterRepository)
        {
            _jobRepository = repo;
            _recruiterRepository = recruiterRepository;
        }

        public async Task<JobDto> AddJobAsync(CreateJobDto createJobDto, int recruiterId)
        {
            var recruiter = await _recruiterRepository.GetByIdAsync(recruiterId);

            if (recruiter == null)
                throw new InvalidOperationException("Recruiter not found.");

            var job = new Job
            {
                Title = createJobDto.Title,
                Description = createJobDto.Description,
                IsActive = true,
                RecruiterId = recruiterId,
                CreatedAt = DateTime.UtcNow
                
            };
            await _jobRepository.AddAsync(job);
            await _jobRepository.SaveChangesAsync();

            return new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                IsActive = job.IsActive
            };
        }

        public async Task<JobDto> GetJobAsync(int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
                throw new InvalidOperationException("Job not found.");
            return new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                IsActive = job.IsActive
            };
        }

        public async Task<IEnumerable<JobDto>> GetAllJobsAsync()
        {
            var jobs = await _jobRepository.GetAllAsync();
            return jobs.Select(job => new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                IsActive = job.IsActive
            });
        }

        public async Task CloseAsync(int jobId, int recruiterId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                throw new InvalidOperationException(
                    "Job not found.");

            if (job.RecruiterId != recruiterId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this job.");

            // Check if the job is already closed
            if (!job.IsActive)
                throw new InvalidOperationException(
                    "Job is already closed.");

            job.IsActive = false;
            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = recruiterId;

            _jobRepository.Update(job);
            await _jobRepository.SaveChangesAsync();
        }
    }
}
