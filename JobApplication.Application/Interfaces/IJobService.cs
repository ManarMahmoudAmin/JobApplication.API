using JobApplication.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJobService
    {
        Task<JobDto> CreateJobAsync(CreateJobDto createJobDto);
    }
}
