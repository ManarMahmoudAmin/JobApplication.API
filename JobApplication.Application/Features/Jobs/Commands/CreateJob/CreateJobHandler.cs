using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commands.CreateJob
{
    internal class CreateJobHandler : IRequestHandler<CreateJobCommand, JobDto>
    {
        private readonly IGenericRepository<Job> _jobRepository;
        private readonly IGenericRepository<Recruiter> _recruiterRepository;

        public CreateJobHandler(IGenericRepository<Job> jobRepository, IGenericRepository<Recruiter> recruiterRepository)
        {
            _jobRepository = jobRepository;
            _recruiterRepository = recruiterRepository;
        }

        public async Task<JobDto> Handle(CreateJobCommand createJobCommand, CancellationToken cancellationToken)
        {
            var recruiter = await _recruiterRepository.GetByIdAsync(createJobCommand.RecruiterId);

            if (recruiter == null)
                throw new InvalidOperationException("Recruiter not found.");

            var job = new Job
            {
                Title = createJobCommand.Title,
                Description = createJobCommand.Description,
                IsActive = true,
                RecruiterId = createJobCommand.RecruiterId
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
    }
}
