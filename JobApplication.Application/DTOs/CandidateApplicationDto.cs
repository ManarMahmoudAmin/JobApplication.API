using JobApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.DTOs
{
    public class CandidateApplicationDto
    {
        public int CandidateId { get; set; }
        public int JobId { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
    }
}
