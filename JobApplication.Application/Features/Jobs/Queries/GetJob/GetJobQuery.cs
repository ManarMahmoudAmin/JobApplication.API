using JobApplication.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Queries.GetJob
{
    public class GetJobQuery : IRequest<JobDto>
    {
        public int Id { get; set; }

    }
}
