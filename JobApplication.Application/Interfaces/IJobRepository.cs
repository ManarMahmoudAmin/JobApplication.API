using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJobRepository
    {
        Task<Job> CreateAsync(Job job);
        Task<Job?> GetByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllAsync();
        Task UpdateAsync(Job job);
        Task DeleteAsync(Job job);       
    }
}
