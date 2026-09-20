using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Repositories
{

    public class ApplicationRepository : GenericRepository<CandidateApplication>, IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int candidateId, int jobId)
        {
            return await _context.CandidateApplications
            .AnyAsync(ca =>
                ca.CandidateId == candidateId &&ca.JobId == jobId);
        }
        public async Task<Job?> GetJobByIdAsync(int jobId)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }
    }
}
