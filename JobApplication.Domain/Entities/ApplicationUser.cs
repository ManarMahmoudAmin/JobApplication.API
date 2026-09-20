using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int? CandidateId { get; set; }
        public int? RecruiterId { get; set; }

        public Candidate? Candidate { get; set; }
        public Recruiter? Recruiter { get; set; }
    }
}
