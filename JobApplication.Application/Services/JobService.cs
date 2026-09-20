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
        private readonly IGenericRepository<Job> _repo;
        private readonly IGenericRepository<Recruiter> _recruiterRepository;

        public JobService(IGenericRepository<Job> repo, IGenericRepository<Recruiter> recruiterRepository)
        {
            _repo = repo;
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
                RecruiterId = recruiterId
            };
            await _repo.AddAsync(job);
            await _repo.SaveChangesAsync();

            return new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                IsActive = job.IsActive
            };
        }
    }
}
