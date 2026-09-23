using JobApplication.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJobService
    {
        Task<JobDto> AddJobAsync(CreateJobDto createJobDto, int recruiterId);
        Task<JobDto> GetJobAsync(int jobId);
        Task<IEnumerable<JobDto>> GetAllJobsAsync();
        Task CloseAsync(int jobId, int recruiterId);
    }
}
