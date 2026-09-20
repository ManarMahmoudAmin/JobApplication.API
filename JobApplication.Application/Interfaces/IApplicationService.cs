using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IApplicationService
    {
        Task Apply(int candidateId, int jobId);
    }
}
