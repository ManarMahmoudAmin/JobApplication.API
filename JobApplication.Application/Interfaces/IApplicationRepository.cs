using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IApplicationRepository : IGenericRepository<CandidateApplication>
    {
        Task<bool> ExistsAsync(int candidateId, int jobId);
        Task<Job?> GetJobByIdAsync(int jobId);
    }
}
