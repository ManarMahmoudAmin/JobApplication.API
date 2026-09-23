using JobApplication.Application.DTOs;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Queries.GetAllJobs
{
    internal class GetAllJobsHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<JobDto>>
    {
        private readonly IGenericRepository<Job> _jobRepository;
        public GetAllJobsHandler(IGenericRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<IEnumerable<JobDto>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
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
    }
}
