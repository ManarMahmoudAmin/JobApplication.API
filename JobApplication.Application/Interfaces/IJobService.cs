using JobApplication.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJobService
    {
        Task<JobDto> AddJobAsync(CreateJobDto createJobDto, int recruiterId);
        Task CloseAsync(int jobId, int recruiterId);
    }
}
