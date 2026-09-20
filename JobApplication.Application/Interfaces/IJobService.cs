using JobApplication.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    internal interface IJobService
    {
        Task<JobDto> CreateJobAsync(CreateJobDto createJobDto);
    }
}
