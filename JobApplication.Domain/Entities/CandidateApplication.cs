using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JobApplication.Domain.Entities
{
    internal class CandidateApplication
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
=        public Candidate Candidate { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
    }
}
